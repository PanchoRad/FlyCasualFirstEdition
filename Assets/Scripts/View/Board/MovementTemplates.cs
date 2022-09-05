using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BoardTools;
using Ship;
using Remote;
using Movement;

public static class MovementTemplates {

	private static Vector3 savedRulerPosition;
	private static Vector3 savedRulerRotation;
    private static List<Vector3> rulerCenterPoints = new List<Vector3>();

    private static Transform Templates;
    public static Transform CurrentTemplate;

    public static void PrepareMovementTemplates()
    {
        Templates = GameObject.Find("SceneHolder/Board/RulersHolder").transform;
    }

    public static void AddRulerCenterPoint(Vector3 point)
    {
        rulerCenterPoints.Add(point);
    }

    public static void ResetRuler()
    {
        rulerCenterPoints = new List<Vector3>();
        HideLastMovementRuler();
    }

    public static Vector3 FindNearestRulerCenterPoint(Vector3 pointShipStand)
    {
        Vector3 result = Vector3.zero;
        float minDistance = float.MaxValue;
        foreach (Vector3 rulerCenterPoint in rulerCenterPoints)
        {
            float currentDistance = Vector3.Distance(rulerCenterPoint, pointShipStand);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                result = rulerCenterPoint;
            }
        }

        return result;
    }

    public static void ApplyMovementRuler(GenericShip thisShip) {

        if ((Selection.ThisShip.AssignedManeuver.Speed != 0) || (Selection.ThisShip.AssignedManeuver.Direction != ManeuverDirection.Forward))
        {
            if (!thisShip.isHugeShip)
            {
                ApplyMovementRuler(thisShip, thisShip.AssignedManeuver);
            }
            else
            {
                GameObject ShipBase = thisShip.Model.transform.Find("RotationHelper/RotationHelper2/ShipAllParts/ShipBase/").gameObject;
                ApplyMovementRuler(thisShip, thisShip.AssignedManeuver, ShipBase);
            }
        }
	}

    public static void ApplyMovementRuler(GenericShip thisShip, GenericMovement movement)
    {
        CurrentTemplate = GetMovementRuler(movement);

        if (CurrentTemplate != null)
        {
            SaveCurrentMovementRulerPosition();

            CurrentTemplate.position = thisShip.GetPosition();
            CurrentTemplate.eulerAngles = thisShip.GetAngles() + new Vector3(0f, 90f, 0f);
            if (movement.Direction == ManeuverDirection.Left)
            {
                CurrentTemplate.eulerAngles = CurrentTemplate.eulerAngles + new Vector3(180f, 0f, 0f);
            }
        }
    }

   public static void ApplyMovementRuler(GenericShip thisShip, GenericMovement movement, GameObject ShipBase)
    {
        // FG Huge Ships Movement
        CurrentTemplate = GetMovementRuler(movement);

        if (CurrentTemplate != null)
        {
            SaveCurrentMovementRulerPosition();
            Vector3 BaseReferencePt;
            switch (movement.Direction)
            {
                case ManeuverDirection.Forward:
                    if (thisShip.Model.transform.InverseTransformPoint(0f, 0f, 0f).x < 0)
                    {   // place template tool on left side
                        BaseReferencePt = ShipBase.transform.Find("TemplateLoc_CL_LH").position;
                        CurrentTemplate.position = BaseReferencePt;
                        CurrentTemplate.eulerAngles = thisShip.GetAngles() + new Vector3(0f, 90f, 0f);
                        CurrentTemplate.position = CurrentTemplate.transform.TransformPoint(-CurrentTemplate.transform.Find("StraightMove").localPosition.x, 0f,
                                                                                            -CurrentTemplate.transform.Find("StraightMove").localPosition.z);
                    }
                    else
                    {   // place template tool on right side
                        BaseReferencePt = ShipBase.transform.Find("TemplateLoc_CL_RH").position;
                        CurrentTemplate.position = BaseReferencePt;
                        CurrentTemplate.eulerAngles = thisShip.GetAngles() + new Vector3(0f, -90f, 180f);
                        CurrentTemplate.position = CurrentTemplate.transform.TransformPoint(-CurrentTemplate.transform.Find("StraightMove").localPosition.x, 0f,
                                                                                            -CurrentTemplate.transform.Find("StraightMove").localPosition.z);
                    }
                    break;

                case ManeuverDirection.Left:
                    BaseReferencePt = ShipBase.transform.Find("TemplateLoc_PV1_LH").position;
                    CurrentTemplate.position = BaseReferencePt;
                    CurrentTemplate.eulerAngles = thisShip.GetAngles() + new Vector3(0f, -90f, 180f);
                    CurrentTemplate.position = CurrentTemplate.transform.TransformPoint(-CurrentTemplate.transform.Find("RotationPivot").localPosition.x, 0f,
                                                                                        -CurrentTemplate.transform.Find("RotationPivot").localPosition.z);
                    CurrentTemplate.transform.RotateAround(CurrentTemplate.transform.Find("RotationPivot").position, Vector3.up, -30.0f);
                    break;
                case ManeuverDirection.Right:
                    BaseReferencePt = ShipBase.transform.Find("TemplateLoc_PV1_RH").position;
                    CurrentTemplate.position = BaseReferencePt;
                    CurrentTemplate.eulerAngles = thisShip.GetAngles() + new Vector3(0f, 90f, 0f);
                    CurrentTemplate.position = CurrentTemplate.transform.TransformPoint(-CurrentTemplate.transform.Find("RotationPivot").localPosition.x, 0f,
                                                                                        -CurrentTemplate.transform.Find("RotationPivot").localPosition.z);
                    CurrentTemplate.transform.RotateAround(CurrentTemplate.transform.Find("RotationPivot").position, Vector3.up, 30.0f);
                    break;
            }
        }
    } 
    public static void SaveCurrentMovementRulerPosition()
    {
        savedRulerPosition = CurrentTemplate.position;
        savedRulerRotation = CurrentTemplate.eulerAngles;
    }

    public static Transform GetMovementRuler(GenericMovement movement)
    {
        ResetRuler();

        Transform result = null;
        if (movement != null)
        {
            if (!movement.TheShip.isHugeShip) {
                switch (movement.Bearing)
                {
                    case ManeuverBearing.Straight:
                        return Templates.Find("straight" + movement.Speed);
                    case ManeuverBearing.Bank:
                        return Templates.Find("bank" + movement.Speed);
                    case ManeuverBearing.SegnorsLoop:
                        return Templates.Find("bank" + movement.Speed);
                    case ManeuverBearing.SideslipBank:
                        return Templates.Find("bank" + movement.Speed);
                    case ManeuverBearing.SegnorsLoopUsingTurnTemplate:
                        return Templates.Find("turn" + movement.Speed);
                    case ManeuverBearing.Turn:
                        return Templates.Find("turn" + movement.Speed);
                    case ManeuverBearing.TallonRoll:
                        return Templates.Find("turn" + movement.Speed);
                    case ManeuverBearing.SideslipTurn:
                        return Templates.Find("turn" + movement.Speed);
                    case ManeuverBearing.KoiogranTurn:
                        return Templates.Find("straight" + movement.Speed);
                    case ManeuverBearing.ReverseStraight:
                        return Templates.Find(((movement.Direction == ManeuverDirection.Forward) ? "straight" : "bank") + movement.Speed);
                    case ManeuverBearing.Stationary:
                        return null;
                }
            }
            else
            {							  
                return Templates.Find("HugeShipTool");								
            }
        }
        return result;
	}

    public static void HideLastMovementRuler()
    {
        if (CurrentTemplate != null)
        {
            CurrentTemplate.position = savedRulerPosition;
            CurrentTemplate.eulerAngles = savedRulerRotation;
        }
	}

    public static void ShowRange(GenericShip thisShip, GenericShip anotherShip)
    {
        ShowRangeRuler(new DistanceInfo(thisShip, anotherShip).MinDistance);
    }

    public static bool ShowFiringArcRange(ShotInfo shotInfo)
    {
        if (shotInfo.IsShotAvailable)
        {
            ShowRangeRuler(shotInfo.MinDistance);
        }
        else
        {
            ShowRangeRuler(shotInfo.NearestFailedDistance);
        }
        return shotInfo.IsShotAvailable;
    }

    public static void ShowRangeRuler(RangeHolder rangeInfo)
    {
        Templates.Find("RangeRuler").position = rangeInfo.Point1;
        Templates.Find("RangeRuler").LookAt(rangeInfo.Point2);
    }

    public static void ShowRangeRuler(Vector3 point1, Vector3 point2)
    {
        Templates.Find("RangeRuler").position = point1;
        Templates.Find("RangeRuler").LookAt(point2);
    }

    public static void ShowRangeRulerR2(Vector3 point1, Vector3 point2)
    {
        Templates.Find("RangeRulerR2").position = point1;
        Templates.Find("RangeRulerR2").LookAt(point2);
    }

    public static void ShowRangeRulerR1(Vector3 point1, Vector3 point2)
    {
        Templates.Find("RangeRulerR1").position = point1;
        Templates.Find("RangeRulerR1").LookAt(point2);
    }

    public static void CallReturnRangeRuler(GenericShip thisShip)
    {
        ReturnRangeRuler();
    }

    public static void ReturnRangeRulers()
    {
        ReturnRangeRulerR1();
        ReturnRangeRulerR2();
        ReturnRangeRuler();
    }

    public static void ReturnRangeRuler()
    {
        Templates.Find("RangeRuler").transform.localPosition = new Vector3(10.4f, 0f, -7.5f);
        Templates.Find("RangeRuler").transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public static void ReturnRangeRulerR2()
    {
        Templates.Find("RangeRulerR2").transform.localPosition = new Vector3(11.5f, 0f, -7.5f);
        Templates.Find("RangeRulerR2").transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public static void ReturnRangeRulerR1()
    {
        Templates.Find("RangeRulerR1").transform.localPosition = new Vector3(12.6f, 0f, -7.5f);
        Templates.Find("RangeRulerR1").transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public static Transform GetMovement1Ruler()
    {
        CurrentTemplate = Templates.Find("straight1");
        SaveCurrentMovementRulerPosition();
        return CurrentTemplate;
    }

    public static Transform GetMovement2Ruler()
    {
        CurrentTemplate = Templates.Find("straight2");
        SaveCurrentMovementRulerPosition();
        return CurrentTemplate;
    }

}

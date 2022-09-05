using Ship;
using Obstacles;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using SubPhases;

namespace RulesList
{
    public class ObstacleLandedRule
    {
        static bool RuleIsInitialized = false;

        public ObstacleLandedRule()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (!RuleIsInitialized)
            {
                GenericShip.OnPositionFinishGlobal += CheckLandedOnObstacle;
                RuleIsInitialized = true;
            }
        }

        public void CheckLandedOnObstacle(GenericShip ship)
        {
            if (!ship.isHugeShip) //FG
            {
                if (ship.IsLandedOnObstacle)
                {
                    List<GenericObstacle> obstacles = new List<GenericObstacle>(ship.ObstaclesLanded);
                    foreach (var obstacle in obstacles)
                    {
                        if (ship.IgnoreObstaclesList.Contains(obstacle)) continue;
                        obstacle.OnLanded(ship);
                    }
                }
            } 
            else
            {              
                GameObject BaseFORE = ship.Model.transform.Find("RotationHelper/RotationHelper2/ShipAllParts/ShipBase/ShipBaseColliderFORE").gameObject;
                GameObject BaseAFT = ship.Model.transform.Find("RotationHelper/RotationHelper2/ShipAllParts/ShipBase/ShipBaseColliderAFT").gameObject;
                ObstaclesStayHugeBases ColliderDetectorFOREbase = BaseFORE.GetComponentInChildren<ObstaclesStayHugeBases>();
                ObstaclesStayHugeBases ColliderDetectorAFTbase = BaseAFT.GetComponentInChildren<ObstaclesStayHugeBases>();

                if ((ColliderDetectorAFTbase.LandedOnAsteroid) || (ColliderDetectorFOREbase.LandedOnAsteroid))
                {
                    List<GenericObstacle> obstacles = ColliderDetectorAFTbase.OverlapedAsteroidsAFT;
                    foreach (var obstacle in obstacles) obstacle.OnLandedHugeShip(ship, 2);
                    obstacles = ColliderDetectorFOREbase.OverlapedAsteroidsFORE;
                    foreach (var obstacle in obstacles) obstacle.OnLandedHugeShip(ship, 1);
                }

                if ((ColliderDetectorAFTbase.LandedOnShip) || (ColliderDetectorFOREbase.LandedOnShip))
                {
                    List<GenericShip> crushedShips = ColliderDetectorFOREbase.OverlapedShipsFORE;
                    foreach (var crushedShip in crushedShips)
                    {
                        Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " landed on and crushed " + crushedShip.PilotInfo.PilotName);
                        Triggers.RegisterTrigger(new Trigger()
                        {
                            Name = "Ship was crushed",
                            TriggerType = TriggerTypes.OnPositionFinish,
                            TriggerOwner = ship.Owner.PlayerNo,
                            EventHandler = DestroyShipCrushed,
                            Skippable = true,
                            Sender = crushedShip
                        });
                        
                        Triggers.RegisterTrigger(new Trigger()
                        {
                            Name = "Ship Crushing, Roll for damage",
                            TriggerType = TriggerTypes.OnPositionFinish,
                            TriggerOwner = ship.Owner.PlayerNo,
                            Sender = ship.Owner.PlayerNo,
                            Skippable = true,
                            EventHandler = delegate { RollForDamageDices((crushedShip.ShipBase.Size == BaseSize.Small)); }
                        });
                        ColliderDetectorAFTbase.OverlapedShipsAFT.Remove(crushedShip);
                    }
                    crushedShips = ColliderDetectorAFTbase.OverlapedShipsAFT;
                    foreach (var crushedShip in crushedShips)
                    {
                        Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " landed on and crushed " + crushedShip.PilotInfo.PilotName);
                        Triggers.RegisterTrigger(new Trigger()
                        {
                            Name = "Ship was crushed",
                            TriggerType = TriggerTypes.OnPositionFinish,
                            TriggerOwner = ship.Owner.PlayerNo,
                            EventHandler = DestroyShipCrushed,
                            Skippable = true,
                            Sender = crushedShip
                        });
                        Triggers.RegisterTrigger(new Trigger()
                        {
                            Name = "Ship Crushing, Roll for damage",
                            TriggerType = TriggerTypes.OnPositionFinish,
                            TriggerOwner = ship.Owner.PlayerNo,
                            Sender = ship.Owner.PlayerNo,
                            Skippable = true,
                            EventHandler = delegate { RollForDamageDices((crushedShip.ShipBase.Size == BaseSize.Small)); } 
                        });
                        ColliderDetectorFOREbase.OverlapedShipsFORE.Remove(crushedShip);
                    }
                }
                // Clear Collision Info
                ColliderDetectorFOREbase.ReInitCollisionInfo();
                ColliderDetectorAFTbase.ReInitCollisionInfo();
            }           
        }
        private void RollForDamageDices(bool SingleDice)
        {
            ShipCrushCheckSubPhase newPhase = (ShipCrushCheckSubPhase)Phases.StartTemporarySubPhaseNew(
                (SingleDice) ?"Damage from crushing small ship": "Damage from crushing larger ship",
                typeof(ShipCrushCheckSubPhase),
                delegate
                {
                    Phases.FinishSubPhase(typeof(ShipCrushCheckSubPhase));
                    Triggers.FinishTrigger();
                });
            newPhase.TheShip = Selection.ThisShip;
            newPhase.Start();
        }
        private void DestroyShipCrushed(object sender, System.EventArgs e)
        {
            GenericShip ship = sender as GenericShip;

            Messages.ShowInfo(ship.PilotInfo.PilotName + " was crushed by Huge Ship!");
            ship.DestroyShipForced(Triggers.FinishTrigger, false);
        }
    }
}


namespace SubPhases
{
    public class ShipCrushCheckSubPhase : DiceRollCheckSubPhase
    {
        private GenericShip prevActiveShip = Selection.ActiveShip;

        public override void Prepare()
        {
            DiceKind = DiceKind.Attack;
            DiceCount = (this.Name.Equals("Damage from crushing small ship")) ? 1 : 2;

            AfterRoll = FinishAction;
            Selection.ActiveShip = TheShip;
        }

        protected override void FinishAction()
        {
            HideDiceResultMenu();
            Selection.ActiveShip = prevActiveShip;

            switch (CurrentDiceRoll.DiceList[0].Side)
            {
                case DieSide.Blank:
                    NoDamage();
                    break;
                case DieSide.Focus:
                    NoDamage();
                    break;
                case DieSide.Success:
                    Messages.ShowErrorToHuman("The ship takes a hit!");
                    SufferDamage();
                    break;
                case DieSide.Crit:
                    Messages.ShowErrorToHuman("The ship takes a critical hit!");
                    SufferDamage();
                    break;
                default:
                    break;
            }
        }

        private void NoDamage()
        {
            Messages.ShowInfoToHuman("No damage");
            CallBack();
        }

        private void SufferDamage()
        {
            DamageSourceEventArgs shipCrushDamage = new DamageSourceEventArgs()
            {
                Source = "Ship Crushing",
                DamageType = DamageTypes.ObstacleCollision
            };

            TheShip.Damage.TryResolveDamage(CurrentDiceRoll.DiceList, shipCrushDamage, CallBack);
        }
    }
}

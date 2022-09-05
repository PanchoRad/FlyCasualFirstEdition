using Obstacles;
using Remote;
using Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesStayHugeBases : MonoBehaviour
{
    public enum Base { none, AFT, FORE, both };
    public Base HugeShipBase = Base.FORE;

    public bool checkCollisions = false;
    public bool LandedOnAsteroid = false;
    public bool LandedOnShip = false;
    public bool LandedOnDevice = false;
    public bool OverlapsAsteroid
    {
        get
        {
            if (HugeShipBase == Base.AFT) { return OverlapedAsteroidsAFT.Count > 0; }
            else if (HugeShipBase == Base.FORE) { return OverlapedAsteroidsFORE.Count > 0; }
            else return false;
        }
    }

    public List<GenericObstacle> OverlapedAsteroidsFORE = new List<GenericObstacle>();
    public List<GenericObstacle> OverlapedAsteroidsAFT = new List<GenericObstacle>();
    public List<Collider> OverlapedDevicesFORE = new List<Collider>();
    public List<Collider> OverlapedDevicesAFT = new List<Collider>();
    public List<GenericShip> OverlapedShipsFORE = new List<GenericShip>();
    public List<GenericShip> OverlapedShipsAFT = new List<GenericShip>();

    private GenericShip theShip;
    public GenericShip TheShip
    {
        get { return theShip ?? Selection.ThisShip; }
        set { theShip = value; }
    }

    public void ReInitCollisionInfo()
    {
        OverlapedShipsAFT.Clear();
        OverlapedDevicesAFT.Clear();
        OverlapedAsteroidsAFT.Clear();
        OverlapedShipsFORE.Clear();
        OverlapedDevicesFORE.Clear();
        OverlapedAsteroidsFORE.Clear();

        LandedOnAsteroid = false;
        LandedOnShip = false;
        LandedOnDevice = false;
    }

    private void OnTriggerEnter(Collider collisionInfo)
    {
        if ((checkCollisions) && (!collisionInfo.CompareTag(this.tag)))
        {
            if (collisionInfo.CompareTag("Obstacle"))
            {
                GenericObstacle obstacle = ObstaclesManager.GetChosenObstacle(collisionInfo.transform.name);
                if ((HugeShipBase == Base.AFT) && (!OverlapedAsteroidsAFT.Contains(obstacle)))
                {
                    OverlapedAsteroidsAFT.Add(obstacle);
                }
                if ((HugeShipBase == Base.FORE) && (!OverlapedAsteroidsFORE.Contains(obstacle)))
                {
                    OverlapedAsteroidsFORE.Add(obstacle);
                }
            }
            else if (collisionInfo.CompareTag("Mine"))
            {
                if ((HugeShipBase == Base.AFT) && (!OverlapedDevicesAFT.Contains(collisionInfo)))
                {
                    OverlapedDevicesAFT.Add(collisionInfo);
                }
                if ((HugeShipBase == Base.FORE) && (!OverlapedDevicesFORE.Contains(collisionInfo)))
                {
                    OverlapedDevicesFORE.Add(collisionInfo);
                }
            }
            else if (collisionInfo.tag.Contains("Ship"))
            {
                GenericShip ship = Roster.GetShipById(collisionInfo.tag);
                if ((HugeShipBase == Base.AFT) && (!OverlapedShipsAFT.Contains(ship)))
                {
                    OverlapedShipsAFT.Add(ship);
                }
                if ((HugeShipBase == Base.FORE) && (!OverlapedShipsFORE.Contains(ship)))
                {
                    OverlapedShipsFORE.Add(ship);
                }
            }
        }
    }

    private void OnTriggerExit(Collider collisionInfo)
    {
        if ((checkCollisions) && (!collisionInfo.CompareTag(this.tag)))
        {
            if (collisionInfo.CompareTag("Obstacle"))
            {
                GenericObstacle obstacle = ObstaclesManager.GetChosenObstacle(collisionInfo.transform.name);
                if ((HugeShipBase == Base.AFT) && (OverlapedAsteroidsAFT.Contains(obstacle)))
                {
                    OverlapedAsteroidsAFT.Remove(obstacle);
                }
                if ((HugeShipBase == Base.FORE) && (OverlapedAsteroidsFORE.Contains(obstacle)))
                {
                    OverlapedAsteroidsFORE.Remove(obstacle);
                }
            }
            else if (collisionInfo.CompareTag("Mine"))
            {
                if (!OverlapedDevicesAFT.Contains(collisionInfo))
                {
                    if ((HugeShipBase == Base.AFT) && (OverlapedDevicesAFT.Contains(collisionInfo)))
                    {
                        OverlapedDevicesAFT.Remove(collisionInfo);
                    }
                    if ((HugeShipBase == Base.FORE) && (OverlapedDevicesFORE.Contains(collisionInfo)))
                    {
                        OverlapedDevicesFORE.Remove(collisionInfo);
                    }
                }
            }
            else if (collisionInfo.tag.Contains("Ship"))
            {
                GenericShip ship = Roster.GetShipById(collisionInfo.tag);
                if ((HugeShipBase == Base.AFT) && (OverlapedShipsAFT.Contains(ship)))
                {
                    OverlapedShipsAFT.Remove(ship);
                }
                if ((HugeShipBase == Base.FORE) && (OverlapedShipsFORE.Contains(ship)))
                {
                    OverlapedShipsFORE.Remove(ship);
                }
            }
        }
    }

    private void OnTriggerStay(Collider collisionInfo)
    {
        if (HugeShipBase == Base.AFT)
        {
            LandedOnAsteroid = (OverlapedAsteroidsAFT.Count > 0);
            LandedOnShip = (OverlapedShipsAFT.Count > 0);
            LandedOnDevice = (OverlapedDevicesAFT.Count > 0);
        }
        if (HugeShipBase == Base.FORE)
        {
            LandedOnAsteroid = (OverlapedAsteroidsFORE.Count > 0);
            LandedOnShip = (OverlapedShipsFORE.Count > 0);
            LandedOnDevice = (OverlapedDevicesFORE.Count > 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Movement;

namespace Ship
{
    public class ManeuverInfo
    {
        public ManeuverHolder Movement;
        public MovementComplexity Complexity;

        public ManeuverInfo(ManeuverSpeed speed, ManeuverDirection direction, ManeuverBearing bearing, MovementComplexity complexity)
        {
            Movement = new ManeuverHolder(speed, direction, bearing);
            Complexity = complexity;
        }

        //FG
        public ManeuverInfo(ManeuverSpeed speed, ManeuverDirection direction, ManeuverBearing bearing, MovementComplexity complexity, MovementEnergy energyGain)
        {
            Movement = new ManeuverHolder(speed, direction, bearing, complexity, energyGain);
            Complexity = complexity;
        }
     }

    public class ShipDialInfo
    {
        public Dictionary<ManeuverHolder, MovementComplexity> PrintedDial { get; private set; }

        public ShipDialInfo(params ManeuverInfo[] printedManeuvers)
        {
            PrintedDial = new Dictionary<ManeuverHolder, MovementComplexity>();
            foreach (ManeuverInfo maneuver in printedManeuvers)
            {
                PrintedDial.Add(maneuver.Movement, maneuver.Complexity);
            }
        }

        public void ChangeManeuverComplexity(ManeuverHolder maneuver, MovementComplexity complexity)
        {
            ManeuverHolder match = PrintedDial.First(m => m.Key.Speed == maneuver.Speed && m.Key.Direction == maneuver.Direction && m.Key.Bearing == maneuver.Bearing).Key;
            PrintedDial[match] = complexity;
        }

        public void RemoveManeuver(ManeuverHolder maneuver)
        {
            ManeuverHolder match = PrintedDial.First(m => m.Key.Speed == maneuver.Speed && m.Key.Direction == maneuver.Direction && m.Key.Bearing == maneuver.Bearing).Key;
            PrintedDial.Remove(match);
        }

        public void AddManeuver(ManeuverHolder maneuver, MovementComplexity complexity)
        {
            PrintedDial.Add(maneuver, complexity);
        }

        public int GetManeuverEnergyGain(ManeuverHolder maneuver)
        {
            ManeuverHolder match = PrintedDial.First(m => m.Key.Speed == maneuver.Speed && m.Key.Direction == maneuver.Direction && m.Key.Bearing == maneuver.Bearing).Key;
            switch (match.EnergyGain)
            {
                case MovementEnergy.Energy0:    return 0;
                case MovementEnergy.Energy1:    return 1;
                case MovementEnergy.Energy2:    return 2;
                case MovementEnergy.Energy3:    return 3;
                default:                        return 0;
            }
        } 
    }
}

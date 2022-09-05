using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using Ship;
using Tokens;
using System.Linq;

namespace Ship
{
    public partial class GenericShip
    {
        public void GainManeuverEnergy(GenericShip ship)
        {           
            GenericMovement maneuver = ship.RevealedManeuver;
            ManeuverHolder movementStruct = new ManeuverHolder(maneuver.ManeuverSpeed, maneuver.Direction, maneuver.Bearing, maneuver.ColorComplexity);
            int EnergyGain = ship.DialInfo.GetManeuverEnergyGain(movementStruct);
            if (ship.State.EnergyCurrent + EnergyGain <= ship.State.EnergyMax)
            {
                ship.State.EnergyCurrent += EnergyGain;
            }
            else
            {
                ship.State.EnergyCurrent = ship.State.EnergyMax;
            }
            Roster.UpdateShipStats(ship);
            Roster.UpdateRosterEnergyIndicators(ship);
        }

    }
}

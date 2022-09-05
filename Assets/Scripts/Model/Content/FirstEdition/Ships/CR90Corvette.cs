using System.Collections;
using System.Collections.Generic;
using Movement;
using Actions;
using ActionsList;
using Arcs;
using Upgrade;
using UnityEngine;

namespace Ship.FirstEdition.CR90Corvette
{
    public class CR90Corvette : GenericShip
    {
        public CR90Corvette() : base()
        {
            ShipInfo = new ShipCardInfo
            (
                "CR90 Corvette",
                BaseSize.HugeDualAft,
                Faction.Rebel,
                new ShipArcsInfo(
                    new ShipArcInfo(ArcType.Front, 0)
                ),
                0, 8, 5,
                new ShipActionsInfo(new ActionInfo(typeof(TargetLockAction))),
                new ShipUpgradesInfo(
                    UpgradeType.Crew,
                    UpgradeType.Cargo,
                    UpgradeType.Team,
                    UpgradeType.Hardpoint),
                energy:5
            );

            IconicPilots = new Dictionary<Faction, System.Type> {
                { Faction.Rebel, typeof(CR90CorvetteFore) }
            };

            ModelInfo = new ShipModelInfo(
                "CR90 Corvette",
                "CR90 Corvette",
                new Vector3(-3.25f, 7.55f, 5.55f),
                3.5f
            );

            DialInfo = new ShipDialInfo(
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal, MovementEnergy.Energy2),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal, MovementEnergy.Energy3),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal, MovementEnergy.Energy2),

                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal, MovementEnergy.Energy1),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal, MovementEnergy.Energy2),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal, MovementEnergy.Energy1),

                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal, MovementEnergy.Energy1),

                new ManeuverInfo(ManeuverSpeed.Speed4, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal, MovementEnergy.Energy0)
            );

            SoundInfo = new ShipSoundInfo(
                new List<string>()
                {
                    "Falcon-Fly1",
                    "Falcon-Fly2",
                    "Falcon-Fly3"
                },
                "Falcon-Fire", 2
            );

            ShipIconLetter = '2';

            HotacManeuverTable = new AI.YT1300Table();
        }
    }
}

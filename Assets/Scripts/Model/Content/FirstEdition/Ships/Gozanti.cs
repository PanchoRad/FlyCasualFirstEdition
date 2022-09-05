using System.Collections;
using System.Collections.Generic;
using Movement;
using Actions;
using ActionsList;
using Arcs;
using Upgrade;
using UnityEngine;

namespace Ship.FirstEdition.GozantiCruiser
{
    public class GozantiCruiser : GenericShip
    {
        public GozantiCruiser() : base()
        {
            ShipInfo = new ShipCardInfo
            (
                "Gozanti-class Cruiser",
                BaseSize.Huge,
                Faction.Imperial,
                new ShipArcsInfo(
                    new ShipArcInfo(ArcType.Front, 0)
                ),
                0, 9, 5,
                new ShipActionsInfo(
                    new ActionInfo(typeof(RecoverAction)),
                    new ActionInfo(typeof(ReinforceAction)),
                    new ActionInfo(typeof(CoordinateAction)),
                    new ActionInfo(typeof(TargetLockAction))
                ),
                new ShipUpgradesInfo(
                    UpgradeType.Title,
                    UpgradeType.Crew,
                    UpgradeType.Crew,
                    UpgradeType.Team,
                    UpgradeType.Cargo,
                    UpgradeType.Cargo,
                    UpgradeType.Hardpoint,
                    UpgradeType.Modification ),
                energy:4
            );

            IconicPilots = new Dictionary<Faction, System.Type> {
                { Faction.Imperial, typeof(GozantiClassCruiser) }
            };

            ModelInfo = new ShipModelInfo(
                "Gozanti Cruiser",
                "Gozanti Cruiser",
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
                        "Slave1-Fly1",
                        "Slave1-Fly2"
                    },
                    "Slave1-Fire", 3
            );

            ShipIconLetter = '4';

            HotacManeuverTable = new AI.GozantiTable();
        }
    }
}

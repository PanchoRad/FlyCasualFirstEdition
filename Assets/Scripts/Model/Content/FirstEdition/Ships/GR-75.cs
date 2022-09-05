using System.Collections;
using System.Collections.Generic;
using Movement;
using Actions;
using ActionsList;
using Arcs;
using Upgrade;
using UnityEngine;

namespace Ship.FirstEdition.GR75
{
    public class GR75 : GenericShip
    {
        public GR75() : base()
        {
            ShipInfo = new ShipCardInfo
            (
                "GR-75 Medium Transport",
                BaseSize.Huge,
                Faction.Rebel,
                new ShipArcsInfo(
                    new ShipArcInfo(ArcType.Front, 0)
                ),
                0, 8, 4,
                new ShipActionsInfo(
                    new ActionInfo(typeof(RecoverAction)),
                    new ActionInfo(typeof(ReinforceAction)),
                    new ActionInfo(typeof(CoordinateAction)),
                    new ActionInfo(typeof(JamAction))
                ),
                new ShipUpgradesInfo(
                    UpgradeType.Title,
                    UpgradeType.Crew,
                    UpgradeType.Crew,
                    UpgradeType.Cargo,
                    UpgradeType.Cargo,
                    UpgradeType.Cargo,
                    UpgradeType.Modification ),
                energy:4
            );

            IconicPilots = new Dictionary<Faction, System.Type> {
                { Faction.Rebel, typeof(GR75MediumTransport) }
            };

            ModelInfo = new ShipModelInfo(
                "GR-75",
                "GR-75",
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

            ShipIconLetter = '1';

            HotacManeuverTable = new AI.YT1300Table();
        }
    }
}

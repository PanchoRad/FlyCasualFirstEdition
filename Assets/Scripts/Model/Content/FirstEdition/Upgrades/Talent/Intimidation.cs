﻿using Ship;
using Upgrade;

namespace UpgradesList.FirstEdition
{
    public class Intimidation : GenericUpgrade
    {
        public Intimidation() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Intimidation",
                UpgradeType.Talent,
                cost: 2,
                abilityType: typeof(Abilities.FirstEdition.IntimidationAbility)
            );
        }        
    }
}

namespace Abilities.FirstEdition
{
    //When you are touching an enemy ship, reduce that ship's agility value by 1.
    public class IntimidationAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.AfterGotNumberOfDefenceDiceGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.AfterGotNumberOfDefenceDiceGlobal -= CheckAbility;
        }

        private void CheckAbility(ref int count)
        {
            if (HostShip.Owner != Combat.Defender.Owner && HostShip.ShipsBumped.Contains(Combat.Defender))
            {
                Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + " on a ship at range 0 causes " + Combat.Defender.PilotInfo.PilotName + " to roll 1 fewer defense die");
                count--;
            }
        }
    }
}
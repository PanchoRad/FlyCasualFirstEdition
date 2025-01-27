﻿using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Upgrade
{
    public class UpgradeCardState
    {
        private readonly GenericUpgrade HostUpgrade;
        private GenericShip HostShip { get { return HostUpgrade.HostShip; } }

        public string Name {
            get {
                string result = HostUpgrade.UpgradeInfo.Name;
                if (UsesCharges) result += " (" + Charges + ")";
                if (HostUpgrade.NamePostfix != null) result = result + " " + HostUpgrade.NamePostfix;
                return result;
            }
        }

        public int Charges { get; private set; }
        public int MaxCharges { get; private set; }
        public bool UsesCharges { get { return MaxCharges > 0; } }

        public bool IsFaceup { get; private set; }

        public UpgradeCardState(GenericUpgrade upgrade)
        {
            HostUpgrade = upgrade;

            IsFaceup = true;
            if (upgrade.UpgradeInfo.WeaponInfo == null)
            {
                Charges = upgrade.UpgradeInfo.Charges;
                MaxCharges = upgrade.UpgradeInfo.Charges;
            }
            else
            {
                Charges = upgrade.UpgradeInfo.WeaponInfo.Charges;
                MaxCharges = upgrade.UpgradeInfo.WeaponInfo.Charges;
            };
        }

        public void Flip(bool isFaceup)
        {
            IsFaceup = isFaceup;
        }

        public void SpendCharges(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpendCharge();
            }
        }

        public void LoseCharge()
        {
            Charges--;
            Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);
        }

        public void SpendCharge()
        {
            Charges--;
            if (Charges < 0) throw new InvalidOperationException("Cannot spend charge when you have none left");

            if (Charges == 0) Roster.ShowUpgradeAsInactive(HostShip, HostUpgrade.UpgradeInfo.Name);

            Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);
        }

        public void RestoreCharge()
        {
            if (Charges < MaxCharges && !HostUpgrade.UpgradeInfo.CannotBeRecharged)
            {
                if (Charges == 0) Roster.ShowUpgradeAsActive(HostShip, HostUpgrade.UpgradeInfo.Name);

                Charges++;

                Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);
            }
        }

        public void RestoreCharges(int count)
        {
            if (!HostUpgrade.UpgradeInfo.CannotBeRecharged)
            {
                if (HostUpgrade.UpgradeInfo.RegensChargesCount > 0)
                {
                    Charges = Math.Min(Charges + count, MaxCharges);
                }
                else if (HostUpgrade.UpgradeInfo.RegensChargesCount < 0)
                {
                    Charges = Math.Max(Charges + count, 0);
                }

                Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);
            }
        }
    }
}

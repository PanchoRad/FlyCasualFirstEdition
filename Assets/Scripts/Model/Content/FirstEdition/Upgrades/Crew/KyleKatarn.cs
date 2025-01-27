﻿using Ship;
using Upgrade;
using UnityEngine;
using System;
using Tokens;
using SubPhases;

namespace UpgradesList.FirstEdition
{
    public class KyleKatarn : GenericUpgrade
    {
        public KyleKatarn() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Kyle Katarn",
                UpgradeType.Crew,
                cost: 3,
                isLimited: true,
                restrictions: new UpgradeCardRestrictions(
                    new FactionRestriction(Faction.Rebel),
                    new BaseSizeRestriction(Ship.BaseSize.Small, Ship.BaseSize.Medium, Ship.BaseSize.Large)),
                abilityType: typeof(Abilities.FirstEdition.KyleKatarnCrewAbility)
            );

            Avatar = new AvatarInfo(Faction.Rebel, new Vector2(42, 1));
        }        
    }
}

namespace Abilities.FirstEdition
{
    public class KyleKatarnCrewAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnTokenIsRemoved += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsRemoved -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, Type tokenType)
        {
            if (tokenType == typeof(Tokens.StressToken))
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsRemoved, AskAssignFocusToken);
            }
        }

        private void AskAssignFocusToken(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                AlwaysUseByDefault,
                AssignFocusToken,
                descriptionLong: "Do you want to assign a Focus Token to your ship?",
                imageHolder: HostUpgrade,
                showAlwaysUseOption: true
            );
        }

        private void AssignFocusToken(object sender, EventArgs e)
        {
            HostShip.Tokens.AssignToken(typeof(FocusToken), DecisionSubPhase.ConfirmDecision);
        }
    }
}
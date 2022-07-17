using Ship;
using System;
using System.Linq;
using Tokens;
using Upgrade;

namespace UpgradesList.FirstEdition
{
    public class HarpoonMissiles : GenericSpecialWeapon
    {
        public HarpoonMissiles() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Harpoon Missiles",
                UpgradeType.Missile,
                cost: 4,
                weaponInfo: new SpecialWeaponInfo(
                    attackValue: 4,
                    minRange: 2,
                    maxRange: 3,
                    requiresToken: typeof(BlueTargetLockToken),
                    discard: true
                ),
                abilityType: typeof(Abilities.FirstEdition.HarpoonMissilesAbility)
            );
        }        
    }
}

namespace Abilities.FirstEdition
{
    public class HarpoonMissilesAbility : GenericAbility
    {
        public override void Initialize(GenericShip host)
        {
            base.Initialize(host);

            IsAppliesConditionCard = true;
        }

        public override void ActivateAbility()
        {
            HostShip.OnShotHitAsAttacker += PlanToApplyHarpoonMissilesCondition;
        }

        public override void DeactivateAbility()
        {
            // Ability is turned off only after full attack is finished
            HostShip.OnCombatDeactivation += DeactivateAbilityPlanned;
        }

        private void DeactivateAbilityPlanned(GenericShip ship)
        {
            HostShip.OnCombatDeactivation -= DeactivateAbilityPlanned;
            HostShip.OnShotHitAsAttacker -= PlanToApplyHarpoonMissilesCondition;
        }

        private void PlanToApplyHarpoonMissilesCondition()
        {
            if (Combat.ChosenWeapon == this.HostUpgrade)
            {
                HostShip.OnAttackFinishAsAttacker += ApplyHarpoonMissilesCondition;

                //Missile was discarded
                HostShip.OnShotHitAsAttacker -= PlanToApplyHarpoonMissilesCondition;
            }
        }

        private void ApplyHarpoonMissilesCondition(GenericShip attacker)
        {
            HostShip.OnAttackFinishAsAttacker -= ApplyHarpoonMissilesCondition;

            Messages.ShowInfo("The \"Harpooned!\" condition has been assigned to " + Combat.Defender.PilotInfo.PilotName);
            Combat.Defender.Tokens.AssignCondition(typeof(Conditions.Harpooned));
        }
    }
}

namespace ActionsList
{

    public class HarpoonedRepairAction : GenericAction
    {
        public HarpoonedRepairAction()
        {
            Name = DiceModificationName = "\"Harpooned!\": Discard condition";
        }

        public override void ActionTake()
        {
            HostShip.Tokens.RemoveCondition(typeof(Conditions.Harpooned));

            Phases.StartTemporarySubPhaseOld(
                "Damage from \"Harpooned!\" condition",
                typeof(SubPhases.HarpoonMissilesCheckSubPhase),
                delegate {
                    Phases.FinishSubPhase(typeof(SubPhases.HarpoonMissilesCheckSubPhase));
                    Phases.CurrentSubPhase.CallBack();
                });
        }

        public override int GetActionPriority()
        {
            int result = 90;

            return result;
        }

    }

}

namespace Conditions
{
    public class Harpooned : Tokens.GenericToken
    {
        GenericShip _ship = null;

        public Harpooned(GenericShip host) : base(host)
        {
            Name = ImageName = "Harpooned Condition";
            Temporary = false;
            Tooltip = "https://raw.githubusercontent.com/guidokessels/xwing-data/master/images/conditions/harpooned.png";
        }

        public override void WhenAssigned()
        {
            SubscribeToHarpoonedConditionEffects();
        }

        public override void WhenRemoved()
        {
            UnsubscribeFromHarpoonedConditionEffects();
        }

        private void SubscribeToHarpoonedConditionEffects()
        {
            Host.OnShotHitAsDefender += CheckUncancelledCrit;
            Host.OnShipIsDestroyed += DoSplashDamageOnDestroyed;
            Host.OnGenerateActions += AddRepairAction;
        }

        private void UnsubscribeFromHarpoonedConditionEffects()
        {
            Host.OnShotHitAsDefender -= CheckUncancelledCrit;
            Host.OnShipIsDestroyed -= DoSplashDamageOnDestroyed;
            Host.OnGenerateActions -= AddRepairAction;
        }

        private void CheckUncancelledCrit()
        {
            if (Combat.DiceRollAttack.CriticalSuccesses > 0)
            {
                Host.Tokens.RemoveCondition(this);
                DoSplashDamage(Host, true);
            }
        }


        private void DoSplashDamage(GenericShip harpoonedShip, bool AdditionalDamageOnItself)
        {
            Messages.ShowInfo("\"Harpooned!\" condition deals splash damage");

            var ships = Roster.AllShips.Select(x => x.Value).ToList();
            Host.OnShipIsDestroyed -= DoSplashDamageOnDestroyed;

            foreach (GenericShip ship in ships)
            {           
                // Defending ship shouldn't suffer additional damage
                if (ship.ShipId == harpoonedShip.ShipId)
                {
                    continue;
                }

                BoardTools.DistanceInfo distanceInfo = new BoardTools.DistanceInfo(harpoonedShip, ship);

                if (distanceInfo.Range < 2)
                {
                    //ship.Damage.TryResolveDamage(1, harpoonconditionDamage, callback);
                    Messages.ShowInfoToHuman(ship.PilotInfo.PilotName + " suffered Splash Damage (range " + distanceInfo.Range + ")");

                    _ship = ship;
                    Triggers.RegisterTrigger(new Trigger() 
                        {
                            Name = "Suffer damage from harpoon splash",
                            TriggerType = TriggerTypes.OnAttackFinish,
                            TriggerOwner = Host.Owner.PlayerNo,
                            EventHandler = SufferHarpoonDamage,
                            EventArgs =  new DamageSourceEventArgs()
                            {
                                Source = "Harpoon Condition",
                                DamageType = DamageTypes.CardAbility
                            }
                         }); 
                }
            }
            if (AdditionalDamageOnItself) 
            {
                Triggers.RegisterTrigger(
                new Trigger()
                {
                    Name = "Harpoon Condition: FaceUp Damage Card",
                    TriggerType = TriggerTypes.OnAttackFinish,
                    TriggerOwner = Host.Owner.PlayerNo,
                    Sender = Host.Owner.PlayerNo,
                    Skippable = true,
                    EventHandler = HarpoonAssignDamageCard
                });

                Messages.ShowInfoToHuman(Host.PilotInfo.PilotName + " suffered Splash Damage (critial)");
            }
        }
        private void HarpoonAssignDamageCard(object sender, System.EventArgs e)
        {
            DamageSourceEventArgs harpoonDamage = new DamageSourceEventArgs()
            {
                Source = "Harpoon Condition",
                DamageType = DamageTypes.CardAbility
            };

            Host.SufferHullDamage(true, harpoonDamage);
        }
        private void SufferHarpoonDamage(object sender, EventArgs e)
        {
            _ship.SufferDamage(sender,e);
        }


        private void DoSplashDamageOnDestroyed(GenericShip harpoonedShip, bool isFled)
        {
            Host.Tokens.RemoveCondition(this);
            if (!isFled) {
                DoSplashDamage(Host, false);
            }
        }

      
        private void AddRepairAction(GenericShip harpoonedShip)
        {
            ActionsList.GenericAction action = new ActionsList.HarpoonedRepairAction()
            {
                ImageUrl = (new Harpooned(harpoonedShip)).Tooltip,
                HostShip = harpoonedShip
            };
            harpoonedShip.AddAvailableAction(action);
        }
    }
}

namespace SubPhases
{

    public class HarpoonMissilesCheckSubPhase : DiceRollCheckSubPhase
    {

        public override void Prepare()
        {
            DiceKind = DiceKind.Attack;
            DiceCount = 1;

            AfterRoll = FinishAction;
        }

        protected override void FinishAction()
        {
            HideDiceResultMenu();

            if (CurrentDiceRoll.DiceList[0].Side == DieSide.Success || CurrentDiceRoll.DiceList[0].Side == DieSide.Crit)
            {
                DamageSourceEventArgs harpoonconditionDamage = new DamageSourceEventArgs()
                {
                    Source = "Harpoon Condition",
                    DamageType = DamageTypes.CardAbility
                };

                Selection.ThisShip.Damage.TryResolveDamage(1, harpoonconditionDamage, Phases.CurrentSubPhase.CallBack);
            }
            else
            {
                CallBack();
            }
        }

    }

}
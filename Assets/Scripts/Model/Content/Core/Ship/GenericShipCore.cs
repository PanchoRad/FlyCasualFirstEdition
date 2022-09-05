﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcs;
using Mods;
using Mods.ModsList;
using Abilities;
using System;
using System.Linq;
using Editions;
using Upgrade;
using Players;

namespace Ship
{

    public interface IModifyPilotSkill
    {
        void ModifyPilotSkill(ref int pilotSkill);
    }

    public interface TIE { } //marker interface for ships that counts as "TIEs", ie. Twin Ion Engine MkII
    public interface AWing { } //marker interface for ships that counts as "A-Wing", ie. Starbird Slash

    public partial class GenericShip : IImageHolder
    {
        public ShipCardInfo ShipInfo;
        public PilotCardInfo PilotInfo;
        public ShipDialInfo DialInfo;
        public ShipModelInfo ModelInfo;

        public CustomizedAi Ai;

        public Faction Faction { get { return (PilotInfo.Faction != Faction.None) ? PilotInfo.Faction : ShipInfo.DefaultShipFaction; } }

        public Faction SubFaction
        {
            get
            {
                if (ShipInfo.SubFaction != Faction.None)
                {
                    return ShipInfo.SubFaction;
                }
                else
                {
                    return Faction;
                }
            }
        }

        public ShipStateInfo State;

        public int ShipId { get; protected set; }
        public GenericPlayer Owner { get; protected set; }

        public string PilotName { get; set; }

        public int TargetLockMinRange { get; protected set; }
        public int TargetLockMaxRange { get; protected set; }

        public void CallAfterGetMaxHull(ref int result)
        {
            if (AfterGetMaxHull != null) AfterGetMaxHull(ref result);
        }

        public GameObject Model { get; protected set; }
        public GameObject InfoPanel { get; protected set;  }

        public GenericShipBase ShipBase { get; protected set; }

        public ArcsHolder ArcsInfo { get; protected set; }
        public SectorsHolder SectorsInfo { get; set; }

        public ShipUpgradeBar UpgradeBar { get; protected set; }
        public ShipActionBar ActionBar { get; protected set; }
        public List<Type> DefaultUpgrades { get; protected set; }

        public TokensManager Tokens { get; protected set; }

        private string pilotNameCanonical;
        public string PilotNameCanonical
        {
            get
            {
                if (!string.IsNullOrEmpty(pilotNameCanonical)) return pilotNameCanonical;

                return Tools.Canonicalize(PilotInfo.PilotName);
            }
            set { pilotNameCanonical = value; }
        }

        private string shipTypeCanonical;
        public string ShipTypeCanonical
        {
            get { return Tools.Canonicalize(ShipInfo.ShipName); }
        }

        public List<GenericAbility> PilotAbilities = new List<GenericAbility>();
        public List<GenericAbility> ShipAbilities = new List<GenericAbility>();

        public GenericShip()
        {
            IconicPilots = new Dictionary<Faction, Type>();
            RequiredMods = new List<Type>();
            Maneuvers = new Dictionary<string, Movement.MovementComplexity>();
            UpgradeBar = new ShipUpgradeBar(this);
            Tokens = new TokensManager(this);
            ActionBar = new ShipActionBar(this);
            Ai = new CustomizedAi(this);
            DefaultUpgrades = new List<Type>();

            TargetLockMinRange = 0;
            TargetLockMaxRange = 3;
        }

        public void InitializeGenericShip(PlayerNo playerNo, int shipId, Vector3 position)
        {
            Owner = Roster.GetPlayer(playerNo);
            ShipId = shipId;
            StartingPosition = position;

            InitializeShip();
            InitializePilot();
            InitializeUpgrades();

            InitializeState();

            InitializeShipModel();

            InitializeRosterPanel();
        }

        protected void InitializeRosterPanel()
        {
            InfoPanel = Roster.CreateRosterInfo(this);
            Roster.UpdateUpgradesPanel(this, this.InfoPanel);
            Roster.SubscribeSelectionByInfoPanel(this.InfoPanel.transform.Find("ShipInfo").gameObject);
            Roster.SubscribeUpgradesPanel(this, this.InfoPanel);
        }

        public virtual void InitializeUpgrades()
        {
            foreach (var slot in UpgradeBar.GetUpgradeSlots())
            {
                slot.TryInstallUpgrade(slot.InstalledUpgrade, this);
            }
        }

        public void InitializeState()
        {
            State = new ShipStateInfo(this);

            State.Initiative = PilotInfo.Initiative;
            State.PilotSkillModifiers = new List<IModifyPilotSkill>();

            State.Firepower = ShipInfo.Firepower;
            State.Agility = ShipInfo.Agility;
            State.HullMax = ShipInfo.Hull;
            State.ShieldsMax = ShipInfo.Shields;
            State.ShieldsCurrent = State.ShieldsMax;
            State.EnergyCurrent = 0;
            State.EnergyMax = ShipInfo.Energy;
            State.MaxForce = PilotInfo.Force;

            State.MaxCharges = PilotInfo.Charges > 0 ? PilotInfo.Charges : ShipInfo.Charges;
            State.RegensCharges = PilotInfo.RegensCharges || ShipInfo.RegensCharges;

            Maneuvers = new Dictionary<string, Movement.MovementComplexity>();
            if (DialInfo != null)
            {
                foreach (var maneuver in DialInfo.PrintedDial)
                {
                    Maneuvers.Add(maneuver.Key.ToString(), maneuver.Value);
                }
            }
        }

        public virtual void InitializeShip()
        {
            InitializePilotForSquadBuilder();

            // FG: Mobile Turrets MOD
            if (ModsManager.Mods[typeof(MobileTurretFEMod)].IsOn)
            {
                // replace TurretPrimaryWeapon by a Single Turret (MOD FE1.5)
                if (ShipInfo.ArcInfo.Arcs.Any(i => i.ArcType == ArcType.TurretPrimaryWeapon))
                {
                    ShipInfo.ArcInfo.Arcs.FindLast(i => i.ArcType == ArcType.TurretPrimaryWeapon).ChangeArcType(ArcType.TurretPrimaryWeapon, ArcType.SingleTurret);
                    ShipInfo.ArcInfo.Arcs.FindLast(i => i.ArcType == ArcType.Front).SetAsPrimaryWeaponArc(false);
                    AddFreeMoveArcAction(this);
                }
            }

            foreach (ShipArcInfo arcInfo in ShipInfo.ArcInfo.Arcs)
            {
                if (arcInfo.Firepower != -1) PrimaryWeapons.Add(new PrimaryWeaponClass(this, arcInfo));
            }

            Damage = new Damage(this);
            ActionBar.Initialize();
            if (this.isHugeShip)
            {
                this.OnPerformActionStepStart += GainManeuverEnergy;
            }
        }
        //---------------											 
        private void PerformFreeFocusAction(object sender, System.EventArgs e) // Added FG (MOD FE1.5)
        {
            this.AskPerformFreeAction(
                new ActionsList.RotateArcAction(),
                Triggers.FinishTrigger,
                this.PilotInfo.PilotName,
                 "[FE1.5] Free Rotate Arc action on all First Edition 360 Turrets Equiped Ships",
                this
            );
        }
        public void AddFreeMoveArcAction(GenericShip theShip)  // Added FG (MOD FE1.5)
        {
            ActionsList.GenericAction action = new ActionsList.RotateArcAction()  //typeof(RotateArcAction)
            {
                //ImageUrl = (new MovableArc(theShip)).Tooltip,
                HostShip = theShip
            };
            theShip.AddAvailableFreeAction(action);
            theShip.Tokens.AssignCondition(typeof(Conditions.FreeRotateArcCondition)); // Condition is added too early ReAssigns it in ShipRoster 
            theShip.OnActionDecisionSubphaseEnd += ApplyFreeRotateArcCondition;
        }
        private void ApplyFreeRotateArcCondition(GenericShip HostShip)  // Added FG (MOD FE1.5)
        {
            Triggers.RegisterTrigger(
                    new Trigger()
                    {
                        Name = HostShip.PilotInfo.PilotName + ": Free Rotate Turret Arc Action",
                        TriggerOwner = HostShip.Owner.PlayerNo,
                        TriggerType = TriggerTypes.OnActionIsPerformed,
                        EventHandler = PerformFreeFocusAction
                    }
                );
        }
        //---------------
        public void InitializeShipModel()
        {
            CreateModel(StartingPosition);
            InitializeSectors();
            InitializeShipBaseArc();
            SetId();
            SetShipInsertImage();
            SetShipSkin(GetModelTransform(), GetSkinTexture());
        }

        protected void SetId()
        {
            SetTagOfChildrenRecursive(Model.transform, "ShipId:" + ShipId.ToString());

            SetIdMarker();
            SetSpotlightMask();
        }

        public void InitializeSectors()
        {
            SectorsInfo = new SectorsHolder(this);
        }

        public void InitializeShipBaseArc()
        {
            ArcsInfo = new ArcsHolder(this);
            foreach (ShipArcInfo arc in ShipInfo.ArcInfo.Arcs)
            {
                // FG:  ADDED MOD   (Mobile Turrets)   
                if (ModsManager.Mods[typeof(MobileTurretFEMod)].IsOn)
                {
                    if (arc.ArcType == ArcType.TurretPrimaryWeapon)
                    {
                        arc.ChangeArcType(ArcType.TurretPrimaryWeapon, ArcType.SingleTurret);
                    }
                }													   
                switch (arc.ArcType)
                {
                    case ArcType.Front:
                        ArcsInfo.Arcs.Add(new ArcFront(ShipBase));
                        break;
                    case ArcType.Rear:
                        ArcsInfo.Arcs.Add(new ArcRear(ShipBase));
                        break;
                    case ArcType.FullFront:
                        ArcsInfo.Arcs.Add(new ArcFullFront(ShipBase));
                        break;
                    case ArcType.SingleTurret:
                        ArcsInfo.Arcs.Add(new ArcSingleTurret(ShipBase));
                        break;
                    case ArcType.DoubleTurret:
                        ArcsInfo.Arcs.Add(new ArcDualTurretA(ShipBase));
                        ArcsInfo.Arcs.Add(new ArcDualTurretB(ShipBase));
                        break;
                    case ArcType.Bullseye:
                        ArcsInfo.Arcs.Add(new ArcBullseye(ShipBase));
                        break;
                    case ArcType.TurretPrimaryWeapon:
                        //TODOREVERT
                        // Primary weapon can be used from outside the arc
                        break;
                    case ArcType.SpecialGhost:
                        ArcsInfo.Arcs.Add(new ArcSpecialGhost(ShipBase));
                        break;
                    default:
                        break;
                }
            }
        }

        public void InitializePilotForSquadBuilder()
        {
            InitializeSquadBuilderAbilities();
            InitializeSlots();
        }

        private void InitializeSquadBuilderAbilities()
        {
            foreach (GenericAbility shipAbility in ShipAbilities)
            {
                shipAbility.InitializeForSquadBuilder(this);
            }
        }

        public virtual void InitializePilot()
        {
            PrepareForceInitialization();
            PrepareChargesInitialization();

            ActivateShipAbilities();
            ActivatePilotAbilities();
        }

        private void PrepareForceInitialization()
        {
            OnGameStart += InitializeForce;
        }

        private void InitializeForce()
        {
            OnGameStart -= InitializeForce;
            State.Force = State.MaxForce;
        }

        private void PrepareChargesInitialization()
        {
            OnGameStart += InitializeCharges;
        }

        private void InitializeCharges()
        {
            OnGameStart -= InitializeCharges;
            SetChargesToMax();
        }

        private void ActivateShipAbilities()
        {
            foreach (var shipAbility in ShipAbilities)
            {
                shipAbility.Initialize(this);
            }
        }

        protected void ActivatePilotAbilities()
        {
            if (PilotInfo.AbilityType != null) PilotAbilities.Add((GenericAbility)Activator.CreateInstance(PilotInfo.AbilityType));

            foreach (var pilotAbility in PilotAbilities)
            {
                pilotAbility.Initialize(this);
            }
        }

        private void InitializeSlots()
        {
            foreach (var slot in ShipInfo.UpgradeIcons.Upgrades)
            {
                UpgradeBar.AddSlot(slot);
            }

            foreach (var slot in PilotInfo.ExtraUpgrades)
            {
                UpgradeBar.AddSlot(slot);
            }
            
            if (DebugManager.FreeMode)
            {
                UpgradeBar.AddSlot(UpgradeType.Omni);
            }
        }

        // STAT MODIFICATIONS

        public void ChangeFirepowerBy(int value)
        {
            if (State != null) State.Firepower += value;
            if (AfterStatsAreChanged != null) AfterStatsAreChanged(this);
        }

        public void ChangeAgilityBy(int value)
        {
            if (State != null) State.Agility += value;
            if (AfterStatsAreChanged != null) AfterStatsAreChanged(this);
        }

        public void ChangeMaxHullBy(int value)
        {
            if (State != null) State.HullMax += value;
            if (AfterStatsAreChanged != null) AfterStatsAreChanged(this);
        }

        public void ChangeShieldBy(int value)
        {
            if (State != null) State.ShieldsCurrent += value;
            if (AfterStatsAreChanged != null) AfterStatsAreChanged(this);
        }

        public void SetTargetLockRange(int min, int max)
        {
            TargetLockMinRange = min;
            TargetLockMaxRange = max;
        }

        // CHARGES

        public void SpendCharges(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpendCharge();
            }
        }

        public void SpendCharge()
        {
            State.Charges--;

            if (State.Charges < 0) throw new InvalidOperationException("Cannot spend charge when you have none left");
        }

        public void LoseCharge()
        {
            State.Charges--;
        }

        public void RemoveCharge(Action callBack)
        {
            // for now this is just an alias of SpendCharge
            SpendCharge();
            callBack();
        }

        public void RestoreCharge()
        {
            if (State.Charges < State.MaxCharges)
            {
                State.Charges++;
            }
        }

        public void SetChargesToMax()
        {
            State.Charges = State.MaxCharges;
        }

        public bool CanEquipForceAlignedCard(ForceAlignment alignment)
        {
            var result = false;

            switch (alignment)
            {
                case ForceAlignment.Light:
                    result = Faction == Faction.Republic ||
                             Faction == Faction.Rebel ||
                             Faction == Faction.Resistance;
                    break;
                case ForceAlignment.Dark:
                    result = Faction == Faction.Separatists ||
                             Faction == Faction.Imperial ||
                             Faction == Faction.FirstOrder ||
                             Faction == Faction.Scum;
                    break;
                default:
                    result = true;
                    break;
            }

            OnForceAlignmentEquipCheck?.Invoke(alignment, ref result);

            return result;
        }
    }

}

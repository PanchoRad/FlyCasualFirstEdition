﻿using Arcs;
using BoardTools;
using Bombs;
using Movement;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tokens;
using UnityEngine;
using Upgrade;

namespace Editions
{
    public class FirstEdition : Edition
    {
        public override string Name { get { return "First Edition"; } }
        public override string NameShort { get { return "FirstEdition"; } }

        public override int MaxPoints { get { return 120; } }
        public override int MinShipsCount { get { return 1; } }
        public override int MaxShipsCount { get { return 8; } }
        public override string CombatPhaseName { get { return "Combat"; } }
        public override Color MovementEasyColor { get { return Color.green; } }
        public override bool CanAttackBumpedTarget { get { return false; } }
        public override MovementComplexity IonManeuverComplexity { get { return MovementComplexity.Normal; } }
        public override string PathToSavedSquadrons { get { return "SavedSquadrons"; } }

        public override string RootUrlForImages { get { return "https://raw.githubusercontent.com/guidokessels/xwing-data/master/images/"; } }
        public override Vector2 UpgradeCardSize { get { return new Vector2(196, 300); }  }
        public override Vector2 UpgradeCardCompactOffset { get { return Vector2.zero; } }
        public override Vector2 UpgradeCardCompactSize { get { return UpgradeCardSize; } }

        public override Dictionary<Type, int> DamageDeckContent
        {
            get
            {
                if (DebugManager.OldDamageDeck)
                {
                    return new Dictionary<Type, int>()
                    {
                        { typeof(DamageDeckCardFE.DirectHit), 7 },
                        { typeof(DamageDeckCardFE.BlindedPilotOld), 2 },
                        { typeof(DamageDeckCardFE.DamagedCockpit), 2 },
                        { typeof(DamageDeckCardFE.DamagedEngine), 2 },
                        { typeof(DamageDeckCardFE.DamagedSensorArrayOld), 2 },
                        { typeof(DamageDeckCardFE.InjuredPilot), 2 },
                        { typeof(DamageDeckCardFE.MinorExplosion), 2 },
                        { typeof(DamageDeckCardFE.MunitionsFailure), 2 },
                        { typeof(DamageDeckCardFE.StructuralDamage), 2 },
                        { typeof(DamageDeckCardFE.ThrustControlFire), 2 },
                        { typeof(DamageDeckCardFE.WeaponMalfunction), 2 },
                        { typeof(DamageDeckCardFE.ConsoleFire), 2 },
                        { typeof(DamageDeckCardFE.StunnedPilot), 2 },
                        { typeof(DamageDeckCardFE.MinorHullBreach), 2 },
                        { typeof(DamageDeckCardFE.MajorExplosion), 2 }
                    };
                }
                else
                {
                    return new Dictionary<Type, int>()
                    {
                        { typeof(DamageDeckCardFE.DirectHit),           7 },
                        { typeof(DamageDeckCardFE.BlindedPilot),        2 },
                        { typeof(DamageDeckCardFE.DamagedCockpit),      2 },
                        { typeof(DamageDeckCardFE.DamagedEngine),       2 },
                        { typeof(DamageDeckCardFE.DamagedSensorArray),  2 },
                        { typeof(DamageDeckCardFE.LooseStabilizer),     2 },
                        { typeof(DamageDeckCardFE.MajorHullBreach),     2 },
                        { typeof(DamageDeckCardFE.ShakenPilot),         2 },
                        { typeof(DamageDeckCardFE.StructuralDamage),    2 },
                        { typeof(DamageDeckCardFE.ThrustControlFire),   2 },
                        { typeof(DamageDeckCardFE.WeaponsFailure),      2 },
                        { typeof(DamageDeckCardFE.ConsoleFire),         2 },
                        { typeof(DamageDeckCardFE.StunnedPilot),        2 },
                        { typeof(DamageDeckCardFE.MinorHullBreach),     2 },
                        { typeof(DamageDeckCardFE.MajorExplosion),      2 }
                    };
                }
            }
        }

        public override Dictionary<BaseSize, int> NegativeTokensToAffectShip {
            get
            {
                return new Dictionary<BaseSize, int>()
                {
                    { BaseSize.None,     int.MaxValue },
                    { BaseSize.Small,    1 },
                    { BaseSize.Medium,   2 },   //FG MOD FE1.5
                    { BaseSize.Large,    2 },
                    { BaseSize.Huge,     10 },  //FG Huge ship does not get ionised
                    { BaseSize.HugeDualAft, 10 },
                    { BaseSize.HugeDualFore, 10 }
                };
            }
        }

        public override Dictionary<string, string> PreGeneratedAiSquadrons
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "BlueSquadron",       "{\"name\":\"Blue Squadron\",\"faction\":\"rebel\",\"points\":100,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"bluesquadronnovice\",\"points\":25,\"ship\":\"t70xwing\",\"upgrades\":{\"mod\":[\"integratedastromech\"],\"amd\":[\"r5astromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Blue\"}}},{\"name\":\"bluesquadronnovice\",\"points\":25,\"ship\":\"t70xwing\",\"upgrades\":{\"mod\":[\"integratedastromech\"],\"amd\":[\"r5astromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Blue\"}}},{\"name\":\"bluesquadronnovice\",\"points\":25,\"ship\":\"t70xwing\",\"upgrades\":{\"mod\":[\"integratedastromech\"],\"amd\":[\"r5astromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Blue\"}}},{\"name\":\"bluesquadronnovice\",\"points\":25,\"ship\":\"t70xwing\",\"upgrades\":{\"mod\":[\"integratedastromech\"],\"amd\":[\"r5astromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Blue\"}}}],\"description\":\"Blue Squadron Novice + Integrated Astromech + R5 Astromech\nBlue Squadron Novice + Integrated Astromech + R5 Astromech\nBlue Squadron Novice + Integrated Astromech + R5 Astromech\nBlue Squadron Novice + Integrated Astromech + R5 Astromech\"}" },
                    { "ScurrgAlpha",        "{\"name\":\"Scurrg Alpha\",\"faction\":\"scum\",\"points\":99,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"lokrevenant\",\"points\":33,\"ship\":\"scurrgh6bomber\",\"upgrades\":{\"mod\":[\"guidancechips\"],\"turret\":[\"autoblasterturret\"],\"missile\":[\"harpoonmissiles\"],\"ept\":[\"deadeye\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Lok Revenant\"}}},{\"name\":\"lokrevenant\",\"points\":33,\"ship\":\"scurrgh6bomber\",\"upgrades\":{\"mod\":[\"guidancechips\"],\"turret\":[\"autoblasterturret\"],\"missile\":[\"harpoonmissiles\"],\"ept\":[\"deadeye\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Lok Revenant\"}}},{\"name\":\"lokrevenant\",\"points\":33,\"ship\":\"scurrgh6bomber\",\"upgrades\":{\"mod\":[\"guidancechips\"],\"turret\":[\"autoblasterturret\"],\"missile\":[\"harpoonmissiles\"],\"ept\":[\"deadeye\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Lok Revenant\"}}}],\"description\":\"Lok Revenant + Guidance Chips + Autoblaster Turret + Harpoon Missiles + Deadeye\nLok Revenant + Guidance Chips + Autoblaster Turret + Harpoon Missiles + Deadeye\nLok Revenant + Guidance Chips + Autoblaster Turret + Harpoon Missiles + Deadeye\"}}" },
                    { "StormSquadron",      "{\"name\":\"Storm Squadron\",\"faction\":\"imperial\",\"points\":96,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"stormsquadronpilot\",\"points\":24,\"ship\":\"tieadv\",\"upgrades\":{\"title\":[\"tiex1\"],\"system\":[\"advtargetingcomputer\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"name\":\"stormsquadronpilot\",\"points\":24,\"ship\":\"tieadv\",\"upgrades\":{\"title\":[\"tiex1\"],\"system\":[\"advtargetingcomputer\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"name\":\"stormsquadronpilot\",\"points\":24,\"ship\":\"tieadv\",\"upgrades\":{\"title\":[\"tiex1\"],\"system\":[\"advtargetingcomputer\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"name\":\"stormsquadronpilot\",\"points\":24,\"ship\":\"tieadv\",\"upgrades\":{\"title\":[\"tiex1\"],\"system\":[\"advtargetingcomputer\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}],\"description\":\"Storm Squadron Pilot + TIE/x1 + Adv. Targeting Computer\nStorm Squadron Pilot + TIE/x1 + Adv. Targeting Computer\nStorm Squadron Pilot + TIE/x1 + Adv. Targeting Computer\nStorm Squadron Pilot + TIE/x1 + Adv. Targeting Computer\"}" },
                    { "TacticalWookiees",   "{\"name\":\"Tactical Wookiees\",\"faction\":\"rebel\",\"points\":98,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"wookieeliberator\",\"points\":32,\"ship\":\"auzituckgunship\",\"upgrades\":{\"crew\":[\"tactician\"],\"ept\":[\"expertise\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Kashyyyk Defender\"}}},{\"name\":\"lowhhrick\",\"points\":34,\"ship\":\"auzituckgunship\",\"upgrades\":{\"crew\":[\"tactician\"],\"ept\":[\"expertise\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Lowhhrick\"}}},{\"name\":\"wookieeliberator\",\"points\":32,\"ship\":\"auzituckgunship\",\"upgrades\":{\"crew\":[\"tactician\"],\"ept\":[\"expertise\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Kashyyyk Defender\"}}}],\"description\":\"Wookiee Liberator + Tactician + Expertise\nLowhhrick + Tactician + Expertise\nWookiee Liberator + Tactician + Expertise\"}"},
                    { "ThugLife",           "{\"name\":\"Thug Life\",\"faction\":\"scum\",\"points\":100,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"syndicatethug\",\"points\":25,\"ship\":\"ywing\",\"upgrades\":{\"turret\":[\"twinlaserturret\"],\"samd\":[\"unhingedastromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Brown\"}}},{\"name\":\"syndicatethug\",\"points\":25,\"ship\":\"ywing\",\"upgrades\":{\"turret\":[\"twinlaserturret\"],\"samd\":[\"unhingedastromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Brown\"}}},{\"name\":\"syndicatethug\",\"points\":25,\"ship\":\"ywing\",\"upgrades\":{\"turret\":[\"twinlaserturret\"],\"samd\":[\"unhingedastromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Brown\"}}},{\"name\":\"syndicatethug\",\"points\":25,\"ship\":\"ywing\",\"upgrades\":{\"turret\":[\"twinlaserturret\"],\"samd\":[\"unhingedastromech\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Brown\"}}}],\"description\":\"Syndicate Thug + Twin Laser Turret + Unhinged Astromech\nSyndicate Thug + Twin Laser Turret + Unhinged Astromech\nSyndicate Thug + Twin Laser Turret + Unhinged Astromech\nSyndicate Thug + Twin Laser Turret + Unhinged Astromech\"}" },
                    { "TripleDefenders",    "{\"name\":\"Triple Defenders\",\"faction\":\"imperial\",\"points\":99,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"glaivesquadronpilot\",\"points\":33,\"ship\":\"tiedefender\",\"upgrades\":{\"title\":[\"tiex7\"],\"ept\":[\"veteraninstincts\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Crimson\"}}},{\"name\":\"glaivesquadronpilot\",\"points\":33,\"ship\":\"tiedefender\",\"upgrades\":{\"title\":[\"tiex7\"],\"ept\":[\"veteraninstincts\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Crimson\"}}},{\"name\":\"glaivesquadronpilot\",\"points\":33,\"ship\":\"tiedefender\",\"upgrades\":{\"title\":[\"tiex7\"],\"ept\":[\"veteraninstincts\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Crimson\"}}}],\"description\":\"Glaive Squadron Pilot + TIE/x7 + Veteran Instincts\nGlaive Squadron Pilot + TIE/x7 + Veteran Instincts\nGlaive Squadron Pilot + TIE/x7 + Veteran Instincts\"}" }
                };
            }
        }

        public override int MinShipCost(Faction faction)
        {
            return 12;
        }

        public override void EvadeDiceModification(DiceRoll diceRoll)
        {
            diceRoll.AddDiceAndShow(DieSide.Success);
        }

        public override bool IsWeaponHaveRangeBonus(IShipWeapon weapon)
        {
            return weapon.WeaponType == WeaponTypes.PrimaryWeapon;
        }

        public override void SetShipBaseImage(GenericShip ship)
        {
            ship.SetShipBaseImageFirstEditionV2();
        }

        public override void RotateMobileFiringArc(GenericArc arc, ArcFacing facing)    // FG Added (was in Second Edition)
        {
            arc.ShipBase.Host.ShowMobileFiringArcHighlight(facing);
        }

        public override void RotateMobileFiringArcAlt(GenericArc arc, ArcFacing facing)   // FG Added (was in Second Edition)
        {
            arc.ShipBase.Host.ShowMobileFiringArcAltHighlight(facing);
        }

        public override void BarrelRollTemplatePlanning()
        {
            (Phases.CurrentSubPhase as BarrelRollPlanningSubPhase).PerfromTemplatePlanningFirstEdition();
        }

        public override void DecloakTemplatePlanning()
        {
            (Phases.CurrentSubPhase as DecloakPlanningSubPhase).PerfromTemplatePlanningFirstEdition();
        }

        public override void SubScribeToGenericShipEvents(GenericShip ship)
        {
            ship.OnTryAddAvailableDiceModification += Rules.BullseyeArc.CheckBullseyeArc;
        }

        public override void ReloadAction()
        {
            ActionsList.ReloadAction.FlipFaceupRecursive();
        }

        public override bool DefenderIsReinforcedAgainstAttacker(ArcFacing facing, GenericShip defender, GenericShip attacker)
        {

            ArcType arcType = (facing == ArcFacing.FullFront) ? ArcType.FullFront : ArcType.FullRear;
            return defender.SectorsInfo.IsShipInSector(attacker, arcType);
        }

        public override bool ReinforceEffectCanBeUsed(ArcFacing facing)
        {
            return DefenderIsReinforcedAgainstAttacker(facing, Combat.Defender, Combat.Attacker);
        }

        public override bool ReinforcePostCombatEffectCanBeUsed(ArcFacing facing)
        {
            return false;
        }

        public override void TimedBombActivationTime(GenericShip ship)
        {
            ship.OnManeuverIsRevealed -= BombsManager.CheckBombDropAvailabilityGeneral;
            ship.OnManeuverIsRevealed += BombsManager.CheckBombDropAvailabilityGeneral;
        }

        public override void SquadBuilderIsOpened()
        {
            MainMenu.CurrentMainMenu.ChangePanel("SquadBuilderPanel");
        }

        public override bool IsTokenCanBeDiscardedByJam(GenericToken token)
        {
            List<Type> tokensTypesToDiscard = new List<Type> { typeof(FocusToken), typeof(EvadeToken), typeof(BlueTargetLockToken) };
            return tokensTypesToDiscard.Contains(token.GetType());
        }

        public override string GetPilotImageUrl(GenericShip ship, string filename)
        {
            string URL = RootUrlForImages + "pilots/" + ImageUrls.FormatFaction(ship.SubFaction) + "/" + ImageUrls.FormatShipType(ship.ShipInfo.ShipName) + "/" + (filename ?? (ImageUrls.FormatName(ship.PilotInfo.PilotName) + ".png"));
            return URL;
        }

        public override string GetUpgradeImageUrl(GenericUpgrade upgrade, string filename = null)
        {
            string URL= RootUrlForImages
                + "upgrades/" + ImageUrls.FormatUpgradeTypes(upgrade.UpgradeInfo.UpgradeTypes)
                + "/" + ImageUrls.FormatName(filename ?? ImageUrls.FormatUpgradeName(upgrade.UpgradeInfo.Name))
                + ".png";
            return URL;
        }

        public override string FactionToXws(Faction faction)
        {
            string result = "";

            switch (faction)
            {
                case Faction.Rebel:
                    result = "rebel";
                    break;
                case Faction.Imperial:
                    result = "imperial";
                    break;
                case Faction.Scum:
                    result = "scum";
                    break;
                default:
                    break;
            }

            return result;
        }

        public override Faction XwsToFaction(string factionXWS)
        {
            Faction result = Faction.None;

            switch (factionXWS)
            {
                case "rebel":
                    result = Faction.Rebel;
                    break;
                case "imperial":
                    result = Faction.Imperial;
                    break;
                case "scum":
                    result = Faction.Scum;
                    break;
                default:
                    break;
            }

            return result;
        }

        public override string UpgradeTypeToXws(UpgradeType upgradeType)
        {
            string result = "";

            switch (upgradeType)
            {
                case UpgradeType.Talent:
                    result = "ept";
                    break;
                case UpgradeType.Device:
                    result = "bomb";
                    break;
                case UpgradeType.Sensor:
                    result = "system";
                    break;
                case UpgradeType.Astromech:
                    result = "amd";
                    break;
                case UpgradeType.SalvagedAstromech:
                    result = "samd";
                    break;
                case UpgradeType.Modification:
                    result = "mod";
                    break;
                default:
                    result = upgradeType.ToString().ToLower();
                    break;
            }

            return result;
        }

        public override UpgradeType XwsToUpgradeType(string upgradeXws)
        {
            UpgradeType result = UpgradeType.Astromech;

            switch (upgradeXws)
            {
                case "ept":
                    result = UpgradeType.Talent;
                    break;
                case "bomb":
                    result = UpgradeType.Device;
                    break;
                case "system":
                    result = UpgradeType.Sensor;
                    break;
                case "amd":
                    result = UpgradeType.Astromech;
                    break;
                case "samd":
                    result = UpgradeType.SalvagedAstromech;
                    break;
                case "mod":
                    result = UpgradeType.Modification;
                    break;
                case "tacticalrelay":
                    result = UpgradeType.TacticalRelay;
                    break;
                default:
                    string capitalizedName = upgradeXws.First().ToString().ToUpper() + upgradeXws.Substring(1);
                    result = (UpgradeType)Enum.Parse(typeof(UpgradeType), capitalizedName);
                    break;
            }

            return result;
        }
    }
}

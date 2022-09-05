using ActionsList;
using Actions;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;


namespace Ship
{
    namespace FirstEdition.CR90Corvette
    {
        public class CR90CorvetteFore : CR90Corvette
        {
            public CR90CorvetteFore() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "CR90 Corvette fore",
                    4,
                    50,
                    isLimited: false,
                    extraUpgradeIcons: new List<UpgradeType>() {UpgradeType.Team, UpgradeType.Cargo },
                    addActions: new List<ActionInfo>() {new ActionInfo(typeof(CoordinateAction)), new ActionInfo(typeof(TargetLockAction)) }
                );
            }
        }
    }
}
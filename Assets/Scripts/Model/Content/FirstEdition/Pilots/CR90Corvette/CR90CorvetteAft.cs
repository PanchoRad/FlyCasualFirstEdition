using ActionsList;
using Actions;
using Ship;
using SubPhases;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.CR90Corvette
    {
        public class CR90CorvetteAft : CR90Corvette
        {
            public CR90CorvetteAft() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "CR90 Corvette aft",
                    0,
                    40,  
                    isLimited: false,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Title, UpgradeType.Modification },
                    addActions: new List<ActionInfo>() { new ActionInfo(typeof(ReinforceAction)), new ActionInfo(typeof(RecoverAction)) }
                );
            }
        }
    }
}
using ActionsList;
using Ship;
using SubPhases;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.GozantiCruiser
    {
        public class GozantiClassCruiser : GozantiCruiser
        {
            public GozantiClassCruiser() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Gozanti-class Cruiser",
                    2,
                    40,
                    isLimited: false
                );
            }
        }
    }
}
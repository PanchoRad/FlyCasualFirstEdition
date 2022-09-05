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
    namespace FirstEdition.GR75
    {
        public class GR75MediumTransport : GR75
        {
            public GR75MediumTransport() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "GR-75 Medium Transport",
                    3,
                    30,
                    isLimited: false
                );
            }
        }
    }
}
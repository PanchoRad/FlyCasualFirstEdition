using Ship;
using System;
using System.Linq;
using Tokens;
using Upgrade;

namespace Conditions
{
    public class FreeRotateArcCondition : GenericToken
    {
        //protected virtual string AbilityDescription => "[FE1.5] Free Rotate Arc action on all First Edition 360 Turrets Equiped Ships";

        public FreeRotateArcCondition(GenericShip host): base(host)
        {
            Name = ImageName = "Free Rotate Arc Action";
            Temporary = false;
            //Tooltip = "https://raw.githubusercontent.com/guidokessels/xwing-data/master/images/conditions/harpooned.png";
        }
    }
}

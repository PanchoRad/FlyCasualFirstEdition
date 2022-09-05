using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class GozantiTable : GenericAiTable
    {

        public GozantiTable() : base()  // to be redone
        {
            FrontManeuversInner.Add("1.R.B");
            FrontManeuversInner.Add("1.L.B");
            FrontManeuversInner.Add("2.R.B");
            FrontManeuversInner.Add("2.L.B");
            FrontManeuversInner.Add("4.F.S");
            FrontManeuversInner.Add("4.F.S");

            FrontManeuversOuter.Add("4.F.S");
            FrontManeuversOuter.Add("4.F.S");
            FrontManeuversOuter.Add("3.F.S");
            FrontManeuversOuter.Add("3.F.S");
            FrontManeuversOuter.Add("2.F.S");
            FrontManeuversOuter.Add("2.F.S");

            FrontSideManeuversInner.Add("1.R.B");
            FrontSideManeuversInner.Add("1.L.B");
            FrontSideManeuversInner.Add("2.L.B");
            FrontSideManeuversInner.Add("2.L.B");
            FrontSideManeuversInner.Add("3.F.S");
            FrontSideManeuversInner.Add("3.F.S");

            FrontSideManeuversOuter.Add("1.F.S");
            FrontSideManeuversOuter.Add("1.F.S");
            FrontSideManeuversOuter.Add("2.R.B");
            FrontSideManeuversOuter.Add("2.R.B");
            FrontSideManeuversOuter.Add("1.R.B");
            FrontSideManeuversOuter.Add("1.R.B");

            SideManeuversInner.Add("2.R.B");
            SideManeuversInner.Add("2.L.B");
            SideManeuversInner.Add("1.L.B");
            SideManeuversInner.Add("1.L.B");
            SideManeuversInner.Add("2.F.S");
            SideManeuversInner.Add("1.F.S");

            SideManeuversOuter.Add("2.F.S");
            SideManeuversOuter.Add("2.R.B");
            SideManeuversOuter.Add("2.R.B");
            SideManeuversOuter.Add("1.R.B");
            SideManeuversOuter.Add("1.R.B");
            SideManeuversOuter.Add("1.R.B");

            BackSideManeuversInner.Add("2.R.B");
            BackSideManeuversInner.Add("2.R.B");
            BackSideManeuversInner.Add("1.R.B");
            BackSideManeuversInner.Add("1.R.B");
            BackSideManeuversInner.Add("1.R.B");
            BackSideManeuversInner.Add("1.L.B");

            BackSideManeuversOuter.Add("1.F.S");
            BackSideManeuversOuter.Add("1.L.B");
            BackSideManeuversOuter.Add("2.R.B");
            BackSideManeuversOuter.Add("2.R.B");
            BackSideManeuversOuter.Add("2.R.B");
            BackSideManeuversOuter.Add("1.R.B");

            BackManeuversInner.Add("1.F.S");
            BackManeuversInner.Add("1.L.B");
            BackManeuversInner.Add("1.R.B");
            BackManeuversInner.Add("1.F.S");
            BackManeuversInner.Add("1.L.B");
            BackManeuversInner.Add("1.R.B");

            BackManeuversOuter.Add("1.F.S");
            BackManeuversOuter.Add("1.L.B");
            BackManeuversOuter.Add("2.R.B");
            BackManeuversOuter.Add("2.L.B");
            BackManeuversOuter.Add("1.L.B");
            BackManeuversOuter.Add("1.R.B");
        }

        public override void AdaptToSecondEdition()
        {
            ReplaceManeuver("3.F.R", "3.L.R");
            ReplaceManeuver("1.L.T", "1.L.B");
            ReplaceManeuver("1.R.T", "1.R.B");

            FrontManeuversInner.Remove("2.R.T");
            FrontManeuversInner.Remove("2.L.T");
            FrontManeuversInner.Add("3.R.T");
            FrontManeuversInner.Add("3.L.T");

            FrontSideManeuversInner.Remove("2.L.B");
            FrontSideManeuversInner.Add("2.L.T");

            FrontSideManeuversOuter.Remove("3.R.B");
            FrontSideManeuversOuter.Remove("3.R.B");
            FrontSideManeuversOuter.Add("3.R.T");
            FrontSideManeuversOuter.Add("3.R.T");

            SideManeuversOuter.Remove("3.R.B");
            SideManeuversOuter.Remove("3.R.B");
            SideManeuversOuter.Add("3.R.T");
            SideManeuversOuter.Add("3.R.T");
        }
    }
}
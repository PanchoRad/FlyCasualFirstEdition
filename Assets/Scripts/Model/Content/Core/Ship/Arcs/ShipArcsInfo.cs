using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcs;

namespace Ship
{
    public class ShipArcInfo
    {
        public ArcType ArcType { get; private set; }
        public int Firepower { get; set; }
        public string Name { get; private set; }
        public bool isPrimaryWeaponArc { get; private set; }  //FG

        public ShipArcInfo(ArcType arcType, int firepower = -1)
        {
            ArcType = arcType;
            Firepower = firepower;
            Name = GetArcName(arcType);
            isPrimaryWeaponArc = true; //FG
        }
        public void SetAsPrimaryWeaponArc(bool PrimaryWeaponArc)
        {
            isPrimaryWeaponArc = PrimaryWeaponArc;
        }

        public void ChangeArcType(ArcType fromArcType, ArcType toArcType) //FG Added for FE1.5 MOD
        {
            if (ArcType == fromArcType) ArcType = toArcType;
        }

        private string GetArcName(ArcType arcType)
        {
            string result = "";

            switch (arcType)
            {
                case ArcType.Front:
                    result = "Front";
                    break;
                case ArcType.Rear:
                    result = "Rear";
                    break;
                case ArcType.FullFront:
                    result = "Full Front";
                    break;
                case ArcType.FullRear:
                    result = "Full Rear";
                    break;
                case ArcType.SingleTurret:
                    result = "Turret";
                    break;
                case ArcType.DoubleTurret:
                    result = "Turret";
                    break;
                case ArcType.Bullseye:
                    result = "Bullseye";
                    break;
                case ArcType.TurretPrimaryWeapon:
                    result = "360";
                    break;
                case ArcType.SpecialGhost:
                    result = "Special";
                    break;
                default:
                    break;
            }

            return result;
        }
    }

    public class ShipArcsInfo
    {
        public List<ShipArcInfo> Arcs { get; private set; }

        public ShipArcsInfo(params ShipArcInfo[] arcs)
        {
            Arcs = arcs.ToList();
        }

        public ShipArcsInfo(ArcType arcType, int firepower)
        {
            Arcs = new List<ShipArcInfo>() { new ShipArcInfo(arcType, firepower) };
        }

        public void ChangeArcType(ArcType fromArcType, ArcType toArcType)
        {
            int index = Arcs.FindIndex(n => n.ArcType == fromArcType);
            int firepower = Arcs[index].Firepower;
            ShipArcInfo newArc = new ShipArcInfo(toArcType, firepower);
            Arcs[index] = newArc;
            // Additional MOD play (v1.5)  FG
            //ArcType = toArcType;
        }

        public bool IsMobileTurretShip()
        {
            return Arcs.Any(a => a.ArcType == ArcType.SingleTurret || a.ArcType == ArcType.DoubleTurret);
        }
    }
}

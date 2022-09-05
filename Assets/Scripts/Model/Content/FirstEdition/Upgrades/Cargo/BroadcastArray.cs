using Upgrade;
using Ship;
using Abilities;
using ActionsList;
using Actions;

namespace UpgradesList.FirstEdition
{
    public class BroadcastArray : GenericUpgrade
    {
        public BroadcastArray() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Broadcast Array",
                UpgradeType.Cargo,
                cost: 2,
                restriction: new ShipRestriction(typeof(Ship.FirstEdition.GozantiCruiser.GozantiCruiser)),
                addAction: new ActionInfo(typeof(JamAction))
            );
        }
    }
}
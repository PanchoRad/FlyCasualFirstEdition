﻿using Actions;
using ActionsList;
using Upgrade;

namespace UpgradesList.FirstEdition
{
    public class EngineUpgrade : GenericUpgrade
    {
        public EngineUpgrade() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Engine Upgrade",
                UpgradeType.Modification,
                cost: 4,
                addAction: new ActionInfo(typeof(BoostAction)),
                restriction: new BaseSizeRestriction(Ship.BaseSize.Small, Ship.BaseSize.Medium, Ship.BaseSize.Large)
            );
        }
    }
}
using TabletopTweaks.Core.Config;

namespace SparsSorcerousSundries.Config
{
    public class AddedContent : IUpdatableSettings
    {
        public bool NewSettingsOffByDefault = false;
        public float vendorCostMultiplier = 1f;
        //public SettingGroup RacialArchetypes = new SettingGroup();  As Example
        public SettingGroup KingmakerItems = new SettingGroup();
        public SettingGroup CustomItems = new SettingGroup();
        public SettingGroup GilmoresGoods = new SettingGroup();


        public float VendorCostMultiplier { get => vendorCostMultiplier; }

        public void Init()
        {
        }

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            var loadedSettings = userSettings as AddedContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            vendorCostMultiplier = loadedSettings.vendorCostMultiplier;
            KingmakerItems.LoadSettingGroup(loadedSettings.KingmakerItems, NewSettingsOffByDefault);
            CustomItems.LoadSettingGroup(loadedSettings.CustomItems, NewSettingsOffByDefault);
            GilmoresGoods.LoadSettingGroup(loadedSettings.GilmoresGoods, NewSettingsOffByDefault);
        }
    }
}

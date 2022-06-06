using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace SparsSorcerousSundries
{
    internal static class UMMSettingsUI
    {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Added Content", () => SettingsTabs.AddedContent())

            );
        }
    }

    internal static class SettingsTabs
    {

        public static void AddedContent()
        {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var AddedContent = Main.SSSContext.AddedContent;
            UI.Div(0, 15);
            using (UI.VerticalScope())
            {
                UI.Toggle("New Settings Off By Default".bold(), ref AddedContent.NewSettingsOffByDefault);
                UI.Space(5);
                UI.Label("Vendor Cost Multiplier");
                UI.Slider(ref AddedContent.vendorCostMultiplier, 0.1f, 2f, 1f, 2, "Multiplier");
                UI.Space(25);

                SetttingUI.SettingGroup("Kingmaker Items", TabLevel, AddedContent.KingmakerItems);
                SetttingUI.SettingGroup("Custom Items", TabLevel, AddedContent.CustomItems);
                SetttingUI.SettingGroup("Quest Rewards", TabLevel, AddedContent.QuestRewards);
                SetttingUI.SettingGroup("Gilmores Goods", TabLevel, AddedContent.GilmoresGoods);

                //SetttingUI.NestedSettingGroup();
            }
        }
    }
}
using LockChest.Common;
using LockChest.Frameworks;
using LockChest.Interface;
using LockChest.Interface.Widgets;
using Menu;
using Menu.Common;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockChest
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        AllPrivateChests gameSave;
        IModHelper helper;
        public override void Entry(IModHelper helper)
        {
            this.helper = helper;
            helper.Events.Display.MenuChanged += Display_MenuChanged;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            Helper.Events.GameLoop.DayEnding += GameLoop_DayEnding; 
            Helper.Events.Player.InventoryChanged += Player_InventoryChanged;

            ChestMenu.Instance.InitInstance(helper);

            this.Config = this.Helper.ReadConfig<ModConfig>();

            if (this.Config.CheckForUpdates)
            {
                new UpdateNotifier(base.Monitor, helper, this.Config.GithubUrlForProjectManifest).Check(base.ModManifest);
            }
        }

        private void GameLoop_DayEnding(object sender, StardewModdingAPI.Events.DayEndingEventArgs e)
        {
            gameSave.SaveEndDay();
        }

        private void Player_InventoryChanged(object sender, StardewModdingAPI.Events.InventoryChangedEventArgs e)
        {
            gameSave.SaveTemp();
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            // TODO: change at the end
            if (!Game1.IsMultiplayer)
            {
                gameSave = new AllPrivateChests(this.helper,this.Config.AmountOfChest);

            }
        }
      

        private void Display_MenuChanged(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e)
        {
            // if closed
            if (e.NewMenu == null && e.OldMenu is ItemGrabMenu)
            {
                ChestMenu.Instance.ClearMenu();
            }
            // if changed
            else if (e.OldMenu is ItemGrabMenu)
            {
                ChestMenu.Instance.ClearMenu();
                ChestMenu.Instance.setRemoveChildrenFlag(true);
            }
            ItemGrabMenu itemGrabMenu;
            if((itemGrabMenu = (e.NewMenu as ItemGrabMenu)) != null) // && ChestMenu.Instance.currentMenu == e.OldMenu
            {
                ChestMenu.Instance.CreateMenu(itemGrabMenu);
                ChestOverlay child = new ChestOverlay(itemGrabMenu,this.gameSave,this.helper);//, this.ChestDataManager, this.ChestFiller, this.ItemDataManager, this.WidgetHost.TooltipManager);
            }
        }



    }
}

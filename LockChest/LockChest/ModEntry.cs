using LockChest.Common;
using LockChest.Frameworks;
using LockChest.Interface;
using LockChest.Interface.Widgets;
using Menu;
using Menu.Common;
using StardewModdingAPI;
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
        private IChestDataManager ChestDataManager;
        private IChestFinder ChestFinder;
        private IChestFiller ChestFiller;
        private IItemDataManager ItemDataManager;
        //private ISaveManager SaveManager;
        private string SaveDirectory;
        private string SavePath;


        public override void Entry(IModHelper helper)
        {
            helper.Events.Display.MenuChanged += Display_MenuChanged;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            ChestMenu.Instance.InitInstance(helper);

            this.Config = this.Helper.ReadConfig<ModConfig>();

            if (this.Config.CheckForUpdates)
            {
                new UpdateNotifier(base.Monitor, helper, this.Config.GithubUrlForProjectManifest).Check(base.ModManifest);
            }
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            ////this.ItemDataManager = new ItemDataManager(base.Monitor);
            //this.ChestDataManager = new ChestDataManager(/*this.ItemDataManager,*/ base.Monitor);
            //this.ChestFiller = new ChestFiller(this.ChestDataManager, base.Monitor);
            //this.ChestFinder = new ChestFinder();
            //this.SaveManager = new SaveManager(base.ModManifest.Version, this.ChestDataManager, this.ChestFinder, this.ItemDataManager);
            //this.SavePath = Path.Combine(this.SaveDirectory, Constants.SaveFolderName + ".json");
            //try
            //{
            //    if (File.Exists(this.SavePath))
            //    {
            //        this.SaveManager.Load(this.SavePath);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    base.Monitor.Log(string.Format("Error loading chest data from {0}", this.SavePath), LogLevel.Error);
            //    base.Monitor.Log(ex.ToString(), LogLevel.Debug);
            //}
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
                ChestOverlay child = new ChestOverlay(itemGrabMenu, ChestMenu.Instance.chest, this.Config);//, this.ChestDataManager, this.ChestFiller, this.ItemDataManager, this.WidgetHost.TooltipManager);
                //ChestMenu.Instance.WidgetHost.RootWidget.AddChild(child);
            }
        }



    }
}

using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValleyMods.CategorizeChests.Framework;
using StardewValleyMods.CategorizeChests.Framework.Persistence;
using StardewValleyMods.CategorizeChests.Interface;
using StardewValleyMods.CategorizeChests.Interface.Widgets;
using StardewValleyMods.Common;

namespace StardewValleyMods.CategorizeChests
{
    public class CategorizeChestsMod : Mod
    {

        private WidgetHost WidgetHost;
        private Config Config;
        private IChestDataManager ChestDataManager;
        private IChestFinder ChestFinder;
        private IChestFiller ChestFiller;
        private IItemDataManager ItemDataManager;
        private ISaveManager SaveManager;
        private string SaveDirectory;
        private string SavePath;


        public override void Entry(IModHelper helper)
        {
            this.Config = base.Helper.ReadConfig<Config>();
            if (this.Config.CheckForUpdates)
            {
                new UpdateNotifier(base.Monitor, helper).Check(base.ModManifest);
            }
            this.SaveDirectory = Path.Combine(base.Helper.DirectoryPath, "savedata");
            if (!Directory.Exists(this.SaveDirectory))
            {
                Directory.CreateDirectory(this.SaveDirectory);
            }

            helper.Events.Display.MenuChanged += Display_MenuChanged;
            helper.Events.GameLoop.Saving += GameLoop_Saving;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.ItemDataManager = new ItemDataManager(base.Monitor);
            this.ChestDataManager = new ChestDataManager(this.ItemDataManager, base.Monitor);
            this.ChestFiller = new ChestFiller(this.ChestDataManager, base.Monitor);
            this.ChestFinder = new ChestFinder();
            this.SaveManager = new SaveManager(base.ModManifest.Version, this.ChestDataManager, this.ChestFinder, this.ItemDataManager);
            this.SavePath = Path.Combine(this.SaveDirectory, Constants.SaveFolderName + ".json");
            try
            {
                if (File.Exists(this.SavePath))
                {
                    this.SaveManager.Load(this.SavePath);
                }
            }
            catch (Exception ex)
            {
                base.Monitor.Log(string.Format("Error loading chest data from {0}", this.SavePath), LogLevel.Error);
                base.Monitor.Log(ex.ToString(), LogLevel.Debug);
            }
        }

        private void GameLoop_Saving(object sender, SavingEventArgs e)
        {
            try
            {
                this.SaveManager.Save(this.SavePath);
            }
            catch (Exception ex)
            {
                base.Monitor.Log(string.Format("Error saving chest data to {0}", this.SavePath), LogLevel.Error);
                base.Monitor.Log(ex.ToString(), LogLevel.Debug);
            }
        }

        private void Display_MenuChanged(object sender, MenuChangedEventArgs e)
        {
            // if closed
            if (e.NewMenu == null && e.OldMenu is ItemGrabMenu)
            {
                this.ClearMenu();
            }
            // if changed
            else if (e.OldMenu is ItemGrabMenu)
            {
                this.ClearMenu();
            }
            ItemGrabMenu itemGrabMenu;
            if ((itemGrabMenu = (e.NewMenu as ItemGrabMenu)) != null)
            {
                this.CreateMenu(itemGrabMenu);
            }
        }

        private void OnGameSaving(object sender, EventArgs e)
        {
            
        }

        private void OnGameLoaded(object sender, EventArgs e)
        {
            
        }


        private void CreateMenu(ItemGrabMenu itemGrabMenu)
        {
            ItemGrabMenu.behaviorOnItemSelect behaviorOnItemGrab = itemGrabMenu.behaviorOnItemGrab;
            Chest chest = ((behaviorOnItemGrab != null) ? behaviorOnItemGrab.Target : null) as Chest;
            if (chest != null)
            {
                this.WidgetHost = new WidgetHost(this.Helper);
                ChestOverlay child = new ChestOverlay(itemGrabMenu, chest, this.Config, this.ChestDataManager, this.ChestFiller, this.ItemDataManager, this.WidgetHost.TooltipManager);
                this.WidgetHost.RootWidget.AddChild<ChestOverlay>(child);
            }
        }

        private void ClearMenu()
        {
            WidgetHost widgetHost = this.WidgetHost;
            if (widgetHost != null)
            {
                widgetHost.Dispose();
            }
            this.WidgetHost = null;
        }

    }
}

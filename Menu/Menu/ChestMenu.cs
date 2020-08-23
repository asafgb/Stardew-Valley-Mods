using Menu.Common;
using Menu.Interfaces;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Menu
{
    public class ChestMenu /*: Mod*/
    {
        private IModHelper Helper;
        private Mutex mut = new Mutex();
        private Config Config;


        public Chest chest { get; private set; }
        public static ChestMenu Instance { get; private set; } = new ChestMenu();
        public WidgetHost WidgetHost { get; private set; }
        private ItemGrabMenu currentMenu;
        private bool AlreadyRemoveChildrenOnce = false;

        public ChestMenu()
        {
            //this.Config = this.Helper.ReadConfig<Config>();

            //if (this.Config.CheckForUpdates)
            //{
            //    new UpdateNotifier(base.Monitor, helper, this.Config.GithubUrlForProjectManifest).Check(base.ModManifest);
            //}
        }

        public void setRemoveChildrenFlag(bool request)
        {
            this.AlreadyRemoveChildrenOnce = request;
        }

        public void InitInstance(IModHelper helper)
        {
            if(this.Helper == null)
            {
                this.Helper = helper;
                
                this.Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            }
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            this.Helper.Events.GameLoop.SaveLoaded -= GameLoop_SaveLoaded;
            this.Helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        private void Display_MenuChanged(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e)
        {
            this.AlreadyRemoveChildrenOnce = false;
        }

        public void CreateMenu(ItemGrabMenu itemGrabMenu)
        {
            mut.WaitOne();

            if (itemGrabMenu != this.currentMenu || this.WidgetHost == null)
            {
                this.currentMenu = itemGrabMenu;

                ItemGrabMenu.behaviorOnItemSelect behaviorOnItemGrab = itemGrabMenu.behaviorOnItemGrab;
                Chest chest = ((behaviorOnItemGrab != null) ? behaviorOnItemGrab.Target : null) as Chest;
                if (chest != null)
                {
                    this.chest = chest;
                    this.WidgetHost = new WidgetHost(this.Helper);
                }
            }

            mut.ReleaseMutex();
        }

        public void ClearMenu()
        {
            mut.WaitOne();
            if (!AlreadyRemoveChildrenOnce)
            {
                WidgetHost widgetHost = this.WidgetHost;
                if (widgetHost != null)
                {
                    widgetHost.Dispose();
                }
                this.WidgetHost = null;
            }
            mut.ReleaseMutex();
        }
    }
}

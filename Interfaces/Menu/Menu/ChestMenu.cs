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


        private Mutex mut = new Mutex();
        private Config Config;


        public Chest chest { get; private set; }
        public static ChestMenu Instance { get; private set; } 
        public WidgetHost WidgetHost { get; private set; }
        private ItemGrabMenu currentMenu;
        private bool AlreadyRemoveChildrenOnce = false;

        public ChestMenu()
        {
        }

        /// <summary>
        /// This must be True after clear the menu once,
        /// Cause if not, other mods at the chest menu wont be set 
        /// more specific: be clean
        /// </summary>
        /// <param name="request"></param>
        public void setRemoveChildrenFlag(bool request)
        {
            this.AlreadyRemoveChildrenOnce = request;
        }

        public void InitInstance(IModHelper helper)
        {
            Instance = this;
            if (Stat.helper == null)
            {
                Stat.helper = helper;

                Stat.helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
                Stat.helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            }
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            Stat.helper.Events.GameLoop.SaveLoaded -= GameLoop_SaveLoaded;
            
            Stat.helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            Stat.helper.Content.AssetEditors.Add(new MiscEditor(Stat.helper));
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
                    this.WidgetHost = new WidgetHost(Stat.helper);
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

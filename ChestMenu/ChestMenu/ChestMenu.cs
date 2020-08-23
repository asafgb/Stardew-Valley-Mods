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
    public class ChestMenu : Mod
    {
        private IModHelper helper;
        private Mutex mut = new Mutex();



        public static ChestMenu Instance { get; private set; } //= new ChestMenu();
        public WidgetHost WidgetHost { get; private set; }
        public ItemGrabMenu currentMenu;

        public void InitInstance(IModHelper helper)
        {
            if(Instance == null)
            {
                this.helper = helper;
                Instance = this;
            }
        }
     
        public override void Entry(IModHelper helper)
        {
            InitInstance(helper);
        }

        public void CreateMenu(ItemGrabMenu itemGrabMenu)
        {
            mut.WaitOne();

            if (itemGrabMenu != currentMenu)
                this.currentMenu = itemGrabMenu;

            ItemGrabMenu.behaviorOnItemSelect behaviorOnItemGrab = itemGrabMenu.behaviorOnItemGrab;
            Chest chest = ((behaviorOnItemGrab != null) ? behaviorOnItemGrab.Target : null) as Chest;
            if (chest != null)
            {
                this.WidgetHost = new WidgetHost(this.Helper);
                //ChestOverlay child = new ChestOverlay(itemGrabMenu, chest, this.Config);//, this.ChestDataManager, this.ChestFiller, this.ItemDataManager, this.WidgetHost.TooltipManager);
                //this.WidgetHost.RootWidget.AddChild<ChestOverlay>(child);
            }

            mut.ReleaseMutex();
        }

        public void ClearMenu()
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

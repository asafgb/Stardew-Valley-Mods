using System;
using ChatCommands.Commands;
using StardewModdingAPI;
namespace ChatCommands.test
{
    class ListenCommand : ICommand
    {
        // Token: 0x06000014 RID: 20 RVA: 0x00002610 File Offset: 0x00000810
        public ListenCommand(IMonitor monitor, ChatCommandsConfig config, NotifyingTextWriter writer)
        {
            this.writer = writer;
            this.monitor = monitor;
            bool listenToConsoleOnStartup = config.ListenToConsoleOnStartup;
            if (listenToConsoleOnStartup)
            {
                this.Handle(null, null);
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002646 File Offset: 0x00000846
        public void Register(ICommandHelper helper)
        {
            helper.Add("listen", "Toggles displaying console output in the in game chat box.", new Action<string, string[]>(this.Handle));
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002666 File Offset: 0x00000866
        private void Handle(string name, string[] args)
        {
            this.writer.ToggleForceNotify();
            this.monitor.Log(this.writer.IsForceNotifying() ? "Listening to console output..." : "Stopped listening to console output.", LogLevel.Info);
        }

        // Token: 0x0400000D RID: 13
        private readonly NotifyingTextWriter writer;

        // Token: 0x0400000E RID: 14
        private readonly IMonitor monitor;
    }
}
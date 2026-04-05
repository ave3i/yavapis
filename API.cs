using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using Yavela;

namespace yourAPI
{
    public static class API
    {
        private static string discord_server = "https://discord.gg/yourserver";

        private static async Task checkInjectStatus()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (IsAttached())
                {
                    Execute("print('[yourAPI] Successfully Attached!')"); // many of you got confused with loadstring. i got back to print.
                    break;
                }
            }
        }
        
        public static void InjectAPI()
        {
            Process.Start(discord_server);
            Main.Attach();
            checkInjectStatus();
        }

        public static List<int> GetRobloxPIDs()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Select(p => p.Id).ToList();
        }

        public static bool IsRobloxOpen()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Length > 0;
        }

        public static void KillRoblox()
        {
            foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
                process.Kill();
        }

        public static void AutoInject()
        {
            if (Process.GetProcessesByName("RobloxPlayerBeta").Length > 0)
            {
                try
                {
                    Process.Start(discord_server);
                    Main.Attach();
                }
                catch
                {
                }
            }
        }
    }
}

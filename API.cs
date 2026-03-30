using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;

namespace yourAPI
{
    public static class API
    {
        private const string DllName = "yav-module.dll";
        private static string discord_server = "https://discord.gg/yourserver";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Connect();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsConnected();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void Execute(string input);

        private static async Task checkInjectStatus()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (IsConnected())
                {
                    Execute("print('yourAPI Injected!')");
                    break;
                }
            }
        }
        
        public static void InjectAPI()
        {
            Process.Start(discord_server);
            Connect();
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
                    Connect();
                }
                catch
                {
                }
            }
        }
    }
}

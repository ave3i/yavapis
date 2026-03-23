using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace yourAPI
{
    public class API
    {
        private Timer _timer;
        private bool _executed = false;

        public const string DllName = "yav-module.dll";

        private const string CustomNotif = @"
game.StarterGui:SetCore(""SendNotification"", {
Title=""[yourAPI]"",
Text=""Injected!"",
Duration=5
})";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Attach();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void Execute([MarshalAs(UnmanagedType.LPWStr)] string input);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool IsAttached();

        public void InjectAPI()
        {
            if (IsAttached())
                return;

            try
            {
                Attach();
                _timer = new Timer(ExecuteInjectNotif, null, 0, 500);
            }
            catch (Exception ex)
            {
            }
        }
        
        private async void ExecuteInjectNotif(object state)
        {
            if (_executed) return;
            
            if (IsAttached())
            {
                _executed = true;
                _timer?.Dispose();
                
                await Task.Delay(1000);
                Execute(CustomNotif);
            }
        }

        public void SetAutoInject(bool enabled)
        {
            if (enabled)
            {
                Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");

                if (processes.Length > 0)
                {
                    InjectAPI();
                }
            }
        }

        public void KillRoblox()
        {
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yourAPI
{

    public static class API
    {
        private const string DLL_NAME = "Yavela.dll";

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void ExecuteScript(string Source, uint PID);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Attach(uint PID);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsAttached();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern uint ReturnRobloxInstancesUserIDs(StringBuilder outBuf, uint bufLen, uint PID);

        // ---

        private static readonly HttpClient _http = new HttpClient();
        private static int[] GetPIDs() 
        {
            Process.GetProcessesByName("RobloxPlayerBeta").Select(p => p.Id).ToArray();
        }

        public static async Task Execute(string source, int pid = 0)
        {
            if (pid == 0)
                foreach (int p in GetPIDs())
                    ExecuteScript(source, (uint)p);
            else
                ExecuteScript(source, (uint)pid);
        }

        private static async Task SetupAutoExec()
        {
            string autoexec = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoExec");
            if (!Directory.Exists(autoexec)) return;

            foreach (string file in Directory.GetFiles(autoexec, "*.lua"))
                await Execute(File.ReadAllText(file));
        }
        
        private static async Task DownloadFiles()
        {
            string version_url = "https://files.yavela.xyz/data/api/current_version.txt";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin", "current_version.txt");
            string link = "https://files.yavela.xyz/data/api/Yavela.dll";
            string pathh = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin", "Yavela.dll");

            using (HttpClient client = new HttpClient())
            {
                string web = (await client.GetStringAsync(version_url)).Trim();

                string local = File.Exists(path)
                    ? File.ReadAllText(path).Trim()
                    : string.Empty;

                if (web == local)
                    return;

                byte[] bytes = await client.GetByteArrayAsync(link);
                File.WriteAllBytes(pathh, bytes);

                File.WriteAllText(path, web);
            }
        }
        
        private static async Task CheckInjectStatus()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (IsAttached())
                {
                    await Execute("print('[yourAPI] Successfully Attached!')");
                    break;
                }
            }
        }

        private static async Task MainInj(int PID = 0)
        {
            await SetupAutoExec();
            await Task.Delay(100);
            await DownloadFiles();

            if (pid == 0)
                foreach (int p in GetPIDs())
                    Attach((uint)p);
            else
                Attach((uint)pid);
        }

        public static async Task AttachWithAPI(int PID = 0)
        {
           await MainInj(PID);
            _ = CheckInjectStatus();
        }

        public static List<ulong> GetUserIDs(int pid = 0)
        {
            uint targetPid = (uint)pid;

            uint needed = ReturnRobloxInstancesUserIDs(IntPtr.Zero, 0, targetPid);
            if (needed == 0)
                return new List<ulong>();

            IntPtr buf = Marshal.AllocHGlobal((int)needed + 1);
            try
            {
                uint written = ReturnRobloxInstancesUserIDs(buf, needed + 1, targetPid);
                if (written == 0)
                    return new List<ulong>();

                string result = Marshal.PtrToStringAnsi(buf, (int)written);
                return result
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => ulong.TryParse(x.Trim(), out var val) ? val : 0)
                    .Where(x => x != 0)
                    .ToList();
            }
            finally
            {
                Marshal.FreeHGlobal(buf);
            }
        }

        public static async void AutoAttach()
        {
            if (!RobloxFUNC.IsRobloxOpen()) return;
            await AttachAPI();
        }
    }
}

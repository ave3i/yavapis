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
        private const string DLL_NAME = "Yavela-Module.dll";

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
            return Process.GetProcessesByName("RobloxPlayerBeta").Select(p => p.Id).ToArray();
        }

        public static async Task Execute(string Source, int PID = 0)
        {
            if (PID == 0)
            {

                foreach (int PIDD in GetPIDs())
                {
                    ExecuteScript(Source, (uint)PIDD);
                }
            }
            else 
            {
                ExecuteScript(Source, (uint)PID);
            }
        }

        private static async Task DownloadFile(string URL, string Dest)
        {
            var Bytes = await _http.GetByteArrayAsync(URL);
            File.WriteAllBytes(Dest, Bytes);
        }

        private static async Task StartCommunication()
        {
            try
            {
                string Bin = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin")).FullName;
                string ModulePath = Path.Combine(Bin, DLL_NAME);
                string DecompilerPath = Path.Combine(Bin, "Decompiler.exe");

                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workspace"));
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoExec"));

                string Version = Path.Combine(Bin, "current_version.txt");
                string VersionURL = "https://files.yavela.xyz/data/api/current_version.txt";

                string LatestVersion = (await _http.GetStringAsync(VersionURL)).Trim();
                string LocalVersion = File.Exists(Version) ? File.ReadAllText(Version).Trim() : "";

                bool Outdated = LocalVersion != LatestVersion;

                if (Outdated || !File.Exists(ModulePath))
                {
                    string zipPath = Path.Combine(Bin, "Components.zip");
                    await DownloadFile("https://files.yavela.xyz/data/api/Components.zip", zipPath);

                    using (var archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            string destPath = Path.Combine(Bin, entry.FullName);
                            if (!string.IsNullOrEmpty(entry.Name))
                                entry.ExtractToFile(destPath, overwrite: true);
                        }
                    }

                    File.Delete(zipPath);
                }

                if (Outdated || !File.Exists(DecompilerPath))
                    await DownloadFile("https://files.yavela.xyz/data/api/Decompiler.exe", DecompilerPath);

                if (Outdated)
                    File.WriteAllText(Version, LatestVersion);
            }
            catch
            {
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
            await StartCommunication();
            if (PID == 0)
            {
                foreach (int PIDD in GetPIDs())
                {
                    Attach((uint)PIDD);
                }

            }
            else
            {
                Attach((uint)PID);
            }
        }

        public static async Task AttachAPI(int PID = 0)
        {
           await MainInj(PID);
            _ = CheckInjectStatus();
        }

        public static List<ulong> GetUserIDs(int PID = 0)
        {
            uint Needed = ReturnRobloxInstancesUserIDs(null, 0, (uint)PID);

            if (Needed == 0)
                return new List<ulong>();

            var Buffer = new StringBuilder((int)Needed);

            ReturnRobloxInstancesUserIDs(Buffer, Needed, (uint)PID);

            string Result = Buffer.ToString();

            return Result.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(X => ulong.TryParse(X, out var Val) ? Val : 0).Where(X => X != 0).ToList();
        }

        public static void AutoAttach()
        {
            if (!RobloxFUNC.IsRobloxOpen()) return;
            AttachAPI();
        }
    }
}

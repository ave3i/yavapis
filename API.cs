using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yourAPI
{
    public static class API
    {
        public enum State
        {
            Idle,
            Attaching,
            Attached,
            Detaching,
            Error
        }

        private static readonly HttpClient _http = new HttpClient();
        private static List<int> attached_pids = new List<int>();

        public static State CurrentState { get; private set; } = State.Idle;

        private static readonly string local_appdata =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Yavela");

        private static void CreateShortcut(string targetDir, string shortcutDir)
        {
            string shortcutPath = Path.Combine(shortcutDir, "Yavela Data.lnk");
            if (File.Exists(shortcutPath)) return;

            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            var shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetDir;
            shortcut.Description = "Yavela AppData Folder";
            shortcut.Save();
        }

        public static async Task Execute(string src, int pid = 0)
        {
            if (pid != 0)
                await _http.PostAsync($"http://127.0.0.1:28931/execute/{pid}", new StringContent(src));
            else
                await _http.PostAsync("http://127.0.0.1:28931/execute", new StringContent(src));
        }

        private static async Task DownloadFile(string url, string dest)
        {
            var bytes = await _http.GetByteArrayAsync(url);
            File.WriteAllBytes(dest, bytes);
        }

        public static async Task StartCommunication()
        {
            try
            {
                string bin = Directory.CreateDirectory(Path.Combine(local_appdata, "Bin")).FullName;
                string workspace = Directory.CreateDirectory(Path.Combine(local_appdata, "Workspace")).FullName;
                string autoexec = Directory.CreateDirectory(Path.Combine(local_appdata, "AutoExec")).FullName;

                string original_dir = AppDomain.CurrentDomain.BaseDirectory;
                CreateShortcut(local_appdata, original_dir);

                string initializerp = Path.Combine(bin, "initializer.exe");
                string decompilerp = Path.Combine(bin, "Decompiler.exe");

                string _version = Path.Combine(bin, "current_version.txt");
                string version_url = "https://files.yavela.xyz/data/api/current_version.txt";

                string remote_version = (await _http.GetStringAsync(version_url)).Trim();
                string local_version = File.Exists(_version) ? File.ReadAllText(_version).Trim() : "";

                bool outdated = local_version != remote_version;

                if (outdated || !File.Exists(initializerp))
                {
                    string zipPath = Path.Combine(bin, "attachment.zip");
                    await DownloadFile("https://files.yavela.xyz/data/api/attachment.zip", zipPath);

                    using (var archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            string destPath = Path.Combine(bin, entry.FullName);
                            if (!string.IsNullOrEmpty(entry.Name))
                                entry.ExtractToFile(destPath, overwrite: true);
                        }
                    }

                    File.Delete(zipPath);
                }

                if (outdated || !File.Exists(decompilerp))
                    await DownloadFile("https://files.yavela.xyz/data/api/Decompiler.exe", decompilerp);

                if (outdated)
                    File.WriteAllText(_version, remote_version);

                var valid_exts = new[] { ".txt", ".lua", ".luau" };

                foreach (var file in Directory.GetFiles(autoexec))
                {
                    if (!valid_exts.Contains(Path.GetExtension(file).ToLower())) continue;
                    string src = File.ReadAllText(file);
                    if (!string.IsNullOrWhiteSpace(src))
                        await Execute(src);
                }
            }
            catch
            {
                CurrentState = State.Error;
            }
        }

        public static Task<bool> IsAttached()
        {
            try
            {
                if (attached_pids.Count > 0)
                {
                    CurrentState = State.Attached;
                    return Task.FromResult(true);
                }

                CurrentState = State.Idle;
                return Task.FromResult(false);
            }
            catch
            {
                CurrentState = State.Error;
                return Task.FromResult(false);
            }
        }

        public static async void Attach(int pid = 0)
        {
            try
            {
                CurrentState = State.Attaching;

                string initializerp = Path.Combine(local_appdata, "Bin", "initializer.exe");
                if (File.Exists(initializerp))
                {
                    await StartCommunication();
                }

                var psi = new ProcessStartInfo
                {
                    FileName = initializerp,
                    Arguments = pid.ToString(),
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(psi);

                attached_pids.Add(pid);
                CurrentState = State.Attached;
            }
            catch
            {
                CurrentState = State.Error;
            }
        }

        public static void AutoAttach()
        {
            if (!IsRobloxOpen()) return;
            Attach();
        }

        public static void StopCommunication()
        {
            try
            {
                CurrentState = State.Detaching;

                Process process = Process.GetProcessesByName("Decompiler").FirstOrDefault();
                if (process != null && !process.HasExited)
                    process.Kill();

                Process usermode = Process.GetProcessesByName("initializer").FirstOrDefault();
                if (usermode != null && !usermode.HasExited)
                    usermode.Kill();

                attached_pids.Clear();
                CurrentState = State.Idle;
            }
            catch
            {
                CurrentState = State.Error;
            }
        }

        private static async Task CheckInjectStatus()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (await IsAttached())
                {
                    await Execute("print('[yourAPI] Successfully Attached!')");
                    break;
                }
            }
        }

        public static void InjectAPI()
        {
            Attach();
            _ = CheckInjectStatus();
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
            foreach (var p in Process.GetProcessesByName("RobloxPlayerBeta"))
                p.Kill();
        }

        public static void AutoInject()
        {
            if (IsRobloxOpen())
            {
                try { Attach(); }
                catch { }
            }
        }
    }
}

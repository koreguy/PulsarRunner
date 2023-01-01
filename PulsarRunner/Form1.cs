using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;

namespace PulsarRunner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);
        short ConvertToShort(int value)
        {
            if (value < Int16.MinValue || value > Int16.MaxValue)
                return 0;

            return (short)value;
        }

        private void ReSync()
        {
            string path = Path.Combine(Application.StartupPath, "timesync.bat");
            using (StreamWriter w = new StreamWriter(path))
            {
                w.WriteLine("w32tm /resync");
                w.Close();

            }

            var proc = System.Diagnostics.Process.Start(path);
            proc.WaitForExit();
            File.Delete(path);
        }
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now.AddYears(-1);
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = ConvertToShort(dt.Year);
            st.wMonth = ConvertToShort(dt.Month);
            st.wDay = ConvertToShort(dt.Day);
            st.wHour = ConvertToShort(dt.Hour);
            st.wMinute = ConvertToShort(dt.Minute);
            st.wSecond = ConvertToShort(dt.Second);

            SetSystemTime(ref st);

            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            filePath += @"\Pulsar Fusion Wireless Mice.lnk";
            Process.Start(filePath);

            bool isPulsarRunning = false;

            while (!isPulsarRunning)
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName.Contains("Pulsar"))
                    {
                        isPulsarRunning = true;
                    }
                }
            }

            ReSync();

            System.Environment.Exit(0);
        }
    }
}

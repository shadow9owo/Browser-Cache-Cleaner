using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace CacheCleaner
{
    public partial class Form1 : Form
    {
        static string broser;
        static string appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
        static string[] detectedbrowsers = { "" };
        static long bytestoclean = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label2, "Cache Size");
            label1.BackColor = System.Drawing.Color.Transparent;
            button1.FlatAppearance.BorderSize = 0;
            label1.BackColor = TransparencyKey;
            List<string> list = new List<string>();
            foreach (string directory in Directory.GetDirectories(appdatapath))
            {
                if (directory.Contains("Mozilla"))
                {
                    list.Add("Firefox");
                }
                if (directory.Contains("Google"))
                {
                    foreach (string directory1 in Directory.GetDirectories(appdatapath + @"\Google"))
                    {
                        if (directory1.Contains(appdatapath + @"\Chrome"))
                        {
                            list.Add("Google Chrome");
                        }
                    }
                }
                if (directory.Contains("Microsoft"))
                {
                    foreach (string dir in Directory.GetDirectories(appdatapath + @"\Microsoft\"))
                    {
                        if (dir.Contains("Edge"))
                        {
                            list.Add("Microsoft Edge");
                        }
                    }
                }
                if (directory.Contains("Opera Software"))
                {
                    foreach (string dir in Directory.GetDirectories(appdatapath + @"\Opera Software\"))
                    {
                        if (dir.Contains("Opera Stable"))
                        {
                            list.Add("Opera");
                        }
                        if (dir.Contains("Opera GX Stable"))
                        {
                            list.Add("Opera GX");
                        }
                    }
                }
            }
            detectedbrowsers = list.ToArray();
            foreach (string a in detectedbrowsers)
            {
                broser = broser + a + " ";
            }
            label1.Text = "detected browsers: " + broser;
            foreach (string a in detectedbrowsers)
            {
                if (a.ToLower() == "microsoft edge")
                {
                    bytestoclean = bytestoclean + folderSize(new DirectoryInfo(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Microsoft\Edge\User Data\Default\Service Worker\CacheStorage"));
                }else if (a.ToLower() == "opera")
                {
                    bytestoclean = bytestoclean + folderSize(new DirectoryInfo(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Opera Software\Opera Stable\Service Worker"));
                }
                else if (a.ToLower() == "opera gx")
                {
                    bytestoclean = bytestoclean + folderSize(new DirectoryInfo(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Opera Software\Opera GX Stable\Service Worker"));
                }
                else if (a.ToLower() == "google chrome")
                {
                    bytestoclean = bytestoclean + folderSize(new DirectoryInfo(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\Service Worker\"));
                }
                else if (a.ToLower() == "firefox")//had to use gpt 4 dis cus dis shit is so unreadable and i wouldnt be able to fix this without it
                {
                    foreach (string directory in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles\"))
                    {
                        if (directory.ToLower().Contains("default"))
                        {
                            foreach (string directory1 in Directory.GetDirectories(directory))
                            {
                                if (directory1.Contains("storage"))
                                {
                                    foreach (string directory2 in Directory.GetDirectories(directory1))
                                    {
                                        if (directory2.ToLower().Contains("192.168.0.1") || directory2.ToLower().Contains("localhost"))
                                        {
                                            bytestoclean += folderSize(new DirectoryInfo(directory2));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                label2.Text = FormatBytes(bytestoclean).ToString();
            }
        }
        public static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return string.Format("{0:0.###} {1}", size, sizes[order]);
        }

        static long folderSize(DirectoryInfo folder)
        {
            long totalSizeOfDir = 0;

            // Get all files into the directory
            FileInfo[] allFiles = folder.GetFiles();

            // Loop through every file and get size of it
            foreach (FileInfo file in allFiles)
            {
                totalSizeOfDir += file.Length;
            }

            // Find all subdirectories
            DirectoryInfo[] subFolders = folder.GetDirectories();

            // Loop through every subdirectory and get size of each
            foreach (DirectoryInfo dir in subFolders)
            {
                totalSizeOfDir += folderSize(dir);
            }

            // Return the total size of folder
            return totalSizeOfDir;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bytestoclean = 0;
            foreach (string a in detectedbrowsers)
            {
                if (a.ToLower() == "microsoft edge")
                {
                    foreach (string file in Directory.GetFiles(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Microsoft\Edge\User Data\Default\Service Worker\CacheStorage"))
                    {
                        File.Delete(file);
                    }
                    try
                    {
                        foreach (string directories in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Microsoft\Edge\User Data\Default\Service Worker\CacheStorage"))
                        {
                            Directory.Delete(directories, true);
                        }
                    }catch
                    {

                    }
                }
                else if (a.ToLower() == "opera")
                {
                    foreach (string file in Directory.GetFiles(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Opera Software\Opera Stable\Service Worker"))
                    {
                        File.Delete(file);
                    }
                    try
                    {
                        foreach (string directories in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Opera Software\Opera Stable\Service Worker"))
                        {
                            Directory.Delete(directories, true);
                        }
                    }
                    catch
                    {

                    }
                }
                else if (a.ToLower() == "opera gx")
                {
                    foreach (string file in Directory.GetFiles(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Opera Software\Opera GX Stable\Service Worker"))
                    {
                        File.Delete(file);
                    }
                    try
                    {
                        foreach (string directories in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Opera Software\Opera GX Stable\Service Worker"))
                        {
                            Directory.Delete(directories, true);
                        }
                    }
                    catch
                    {

                    }
                }
                else if (a.ToLower() == "google chrome")
                {
                    foreach (string file in Directory.GetFiles(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\Service Worker\CacheStorage"))
                    {
                        File.Delete(file);
                    }
                    try
                    {
                        foreach (string directories in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\Local\Google\Chrome\User Data\Default\Service Worker\CacheStorage"))
                        {
                            Directory.Delete(directories, true);
                        }
                    }
                    catch
                    {

                    }
                }
                else if (a.ToLower() == "firefox")//had to use gpt 4 dis cus dis shit is so unreadable and i wouldnt be able to fix this without it
                {
                    foreach (string directory in Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles\"))
                    {
                        if (directory.ToLower().Contains("default"))
                        {
                            foreach (string directory1 in Directory.GetDirectories(directory))
                            {
                                if (directory1.Contains("storage"))
                                {
                                    foreach (string directory2 in Directory.GetDirectories(directory1))
                                    {
                                        if (directory2.ToLower().Contains("192.168.0.1"))
                                        {
                                            foreach (string file in Directory.GetFiles(directory2))
                                            {
                                                File.Delete(file);
                                            }
                                            try
                                            {
                                                foreach (string dir in Directory.GetDirectories(directory2))
                                                {
                                                    foreach (string fileind in Directory.GetFiles(dir))
                                                    {
                                                        File.Delete(fileind);
                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else if (directory2.ToLower().Contains("localhost")) {
                                            foreach (string file in Directory.GetFiles(directory2))
                                            {
                                                File.Delete(file);
                                            }
                                            try
                                            {
                                                foreach (string dir in Directory.GetDirectories(directory2))
                                                {
                                                    foreach (string fileind in Directory.GetFiles(dir))
                                                    {
                                                        File.Delete(fileind);
                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                label2.Text = FormatBytes(bytestoclean).ToString();
            }
        }
    }
}

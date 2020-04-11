using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Windows.Forms;
using WxInjector.Core;
using WxInjector.Core.Models;

namespace WxInjector.Graphics
{

    [SuppressMessage("Globalization", "CA1303")]
    public partial class WnMain : Form
    {

        private Records _user;

        public WnMain()
        {
            InitializeComponent();
            Refresh(null, null);
            var userOnline = Native.InternetGetConnectedState(out _, 0);
            if (userOnline)
                Checker.RunWorkerAsync();
        }

        [SuppressMessage("Design", "CA1031")]
        private void Inject(object sender, EventArgs e)
        {
            var item = LbProcesses.SelectedItem as ProcessItem;
            if (LbProcesses.SelectedItem == null || LbDLLs.SelectedItem == null)
            {
                MessageBox.Show(@"Select a process or DLL first before injecting!", @"WxInjector");
                return;
            }

            if (item != null && item.Process == -1)
            {
                MessageBox.Show(@"This process can't be injected!", @"WxInjector");
                return;
            }
            var result = Injector.Result.InjectionSuccessful;
            try
            {
                var injector = new Injector(item.Process);
                result = injector.Inject(LbDLLs.GetItemText(LbDLLs.SelectedItem));
                injector.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(@"An error has occurred! " + error.Message, @"WxInjector");
            }
            if (result != Injector.Result.InjectionSuccessful)
            {
                switch (result)
                {
                    case Injector.Result.SystemProcessDisallowed:
                        MessageBox.Show(@"Injecting into system process is not allowed!", @"WxInjector");
                        break;
                    case Injector.Result.ObtainingProcessHandleFailed:
                        MessageBox.Show(@"Failed obtaining process handle!", @"WxInjector");
                        break;
                    case Injector.Result.DLLAllocationFailed:
                        MessageBox.Show(@"Memory allocation for DLL failed!", @"WxInjector");
                        break;
                    case Injector.Result.DLLWritingFailed:
                        MessageBox.Show(@"Memory writing for DLL failed!", @"WxInjector");
                        break;
                    case Injector.Result.ObtainingLoaderAddressFailed:
                        MessageBox.Show(@"Failed obtaining library loader address!", @"WxInjector");
                        break;
                    case Injector.Result.ObtainingThreadHandleFailed:
                        MessageBox.Show(@"Failed obtaining thread handle!", @"WxInjector");
                        break;
                    case Injector.Result.UnableReleaseMemory:
                        MessageBox.Show(@"Unable to release memory!", @"WxInjector");
                        break;
                }

                return;
            }

            MessageBox.Show(@"DLL successfully injected into process!", @"WxInjector");
        }

        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Add(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Dynamic Link Library|*.dll"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                LbDLLs.Items.Add(dialog.FileName);
            dialog.Dispose();
        }

        private void Remove(object sender, EventArgs e)
        {
            TbDLL.Text = string.Empty;
            if (LbDLLs.SelectedItem != null)
                LbDLLs.Items.Remove(LbDLLs.SelectedItem);
        }

        private void Clear(object sender, EventArgs e)
        {
            TbDLL.Text = string.Empty;
            LbDLLs.Items.Clear();
        }

        private void Start(object sender, EventArgs e)
        {
            _user = Records.Load();
            if (_user.DLLs == null)
                return;
            foreach (var item in _user.DLLs)
                LbDLLs.Items.Add(item);
        }

        private void Release(object sender, FormClosingEventArgs e)
        {
            if (LbDLLs.Items.Count != 0)
            {
                var list = new List<string>();
                foreach (var item in LbDLLs.Items)
                    list.Add(item.ToString());
                _user.DLLs = list.ToArray();
            }
            else
            {
                var list = new List<string>();
                _user.DLLs = list.ToArray();
            }

            _user.Save();
        }

        [SuppressMessage("Globalization", "CA1304")]
        [SuppressMessage("Globalization", "CA1307")]
        private void FileDrop(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files))
                return;
            foreach (var item in files)
                if (Path.GetExtension(item).ToLower().EndsWith("dll"))
                    LbDLLs.Items.Add(item);
                else
                    MessageBox.Show(item + @" is not a valid DLL file!", @"WxInjector");

        }

        private void FileEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Refresh(object sender, EventArgs e)
        {
            TbProcess.Text = string.Empty;
            LbProcesses.Items.Clear();
            var processes = Process.GetProcesses();
            foreach (var item in processes)
                if (!string.IsNullOrEmpty(item.MainWindowTitle))
                    LbProcesses.Items.Add(new ProcessItem(item.ProcessName + ".exe", item.Id));
        }

        private void ProcessesClick(object sender, MouseEventArgs e)
        {
            switch (LbProcesses.SelectedItem)
            {
                case null:
                    return;
                case ProcessItem item:
                    TbProcess.Text = item.Text;
                    break;
                default:
                    MessageBox.Show(@"Internal code malfunction!", @"WxInjector");
                    break;
            }
        }

        private void DLLsClick(object sender, MouseEventArgs e)
        {
            if (LbDLLs.SelectedItem == null)
                return;
            var item = LbDLLs.SelectedItem;
            TbDLL.Text = LbDLLs.GetItemText(item);
        }

        private void CheckForUpdates(object sender, DoWorkEventArgs e)
        {
            var client = new WebClient();
            var data = client.DownloadString("https://raw.githubusercontent.com/dentolos19/WxInjector/master/VERSION");
            client.Dispose();
            if (Version.Parse(Application.ProductVersion) > Version.Parse(data))
            {
                var result = MessageBox.Show(@"Updates is available! Do you want to download it now?", @"WxInjector", MessageBoxButtons.YesNo);
                if (result == DialogResult.OK)
                    Process.Start("https://github.com/dentolos19/WxInjector/releases");
            }
        }

    }

}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Forms;
using WxInjector.Core;
using WxInjector.Core.Models;

namespace WxInjector.Graphics
{

    [SuppressMessage("Globalization", "CA1303")]
    public partial class WnMain : Form
    {

        private Records User;

        public WnMain()
        {
            InitializeComponent();
            Refresh(null, null);
        }

        [SuppressMessage("Design", "CA1031")]
        private void Inject(object Sender, EventArgs Arguments)
        {
            var Item = LbProcesses.SelectedItem as ProcessItem;
            if (LbProcesses.SelectedItem == null || LbDLLs.SelectedItem == null)
            {
                MessageBox.Show("Select a process or DLL first before injecting!", "WxInjector");
                return;
            }

            if (Item.Process == -1)
            {
                MessageBox.Show("This process can't be injected!", "WxInjector");
                return;
            }

            var Result = Injector.Result.InjectionSuccessful;
            try
            {
                var Injector = new Injector(Item.Process);
                Result = Injector.Inject(LbDLLs.GetItemText(LbDLLs.SelectedItem));
                Injector.Dispose();
            }
            catch (Exception Error)
            {
                MessageBox.Show("An error has occurred! " + Error.Message, "WxInjector");
            }

            if (Result != Injector.Result.InjectionSuccessful)
            {
                switch (Result)
                {
                    case Injector.Result.SystemProcessDisallowed:
                        MessageBox.Show("Injecting into system process is not allowed!", "WxInjector");
                        break;
                    case Injector.Result.ObtainingProcessHandleFailed:
                        MessageBox.Show("Failed obtaining process handle!", "WxInjector");
                        break;
                    case Injector.Result.DLLAllocationFailed:
                        MessageBox.Show("Memory allocation for DLL failed!", "WxInjector");
                        break;
                    case Injector.Result.DLLWritingFailed:
                        MessageBox.Show("Memory writing for DLL failed!", "WxInjector");
                        break;
                    case Injector.Result.ObtainingLoaderAddressFailed:
                        MessageBox.Show("Failed obtaining library loader address!", "WxInjector");
                        break;
                    case Injector.Result.ObtainingThreadHandleFailed:
                        MessageBox.Show("Failed obtaining thread handle!", "WxInjector");
                        break;
                    case Injector.Result.UnableReleaseMemory:
                        MessageBox.Show("Unable to release memory!", "WxInjector");
                        break;
                }
                return;
            }

            MessageBox.Show("DLL successfully injected into process!", "WxInjector");
        }

        private void Exit(object Sender, EventArgs Arguments)
        {
            Application.Exit();
        }

        private void Add(object Sender, EventArgs Arguments)
        {
            var Dialog = new OpenFileDialog
            {
                Filter = "Dynamic Link Library|*.dll"
            };
            if (Dialog.ShowDialog() == DialogResult.OK)
                LbDLLs.Items.Add(Dialog.FileName);
            Dialog.Dispose();
        }

        private void Remove(object Sender, EventArgs Arguments)
        {
            TbDLL.Text = string.Empty;
            if (LbDLLs.SelectedItem != null)
                LbDLLs.Items.Remove(LbDLLs.SelectedItem);
        }

        private void Clear(object Sender, EventArgs Arguments)
        {
            TbDLL.Text = string.Empty;
            LbDLLs.Items.Clear();
        }

        private void Start(object Sender, EventArgs Arguments)
        {
            User = Records.Load();
            if (User.DLLs != null)
                foreach (var Item in User.DLLs)
                    LbDLLs.Items.Add(Item);
        }

        private void Release(object Sender, FormClosingEventArgs Arguments)
        {
            if (LbDLLs.Items.Count != 0)
            {
                var List = new List<string>();
                foreach (var Item in LbDLLs.Items)
                    List.Add(Item.ToString());
                User.DLLs = List.ToArray();
            }
            else
            {
                var List = new List<string>();
                User.DLLs = List.ToArray();
            }

            User.Save();
        }

        [SuppressMessage("Globalization", "CA1304")]
        [SuppressMessage("Globalization", "CA1307")]
        private void FileDrop(object Sender, DragEventArgs Arguments)
        {
            var Files = Arguments.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (var Item in Files)
                if (Path.GetExtension(Item).ToLower().EndsWith("dll"))
                    LbDLLs.Items.Add(Item);
                else
                    MessageBox.Show(Item + " is not a valid DLL file!", "WxInjector");
        }

        private void FileEnter(object Sender, DragEventArgs Arguments)
        {
            if (Arguments.Data.GetDataPresent(DataFormats.FileDrop))
                Arguments.Effect = DragDropEffects.Copy;
        }

        private void Refresh(object Sender, EventArgs Arguments)
        {
            TbProcess.Text = string.Empty;
            LbProcesses.Items.Clear();
            var Processes = Process.GetProcesses();
            foreach (var Item in Processes)
                if (!string.IsNullOrEmpty(Item.MainWindowTitle))
                    LbProcesses.Items.Add(new ProcessItem(Item.ProcessName + ".exe", Item.Id));
        }

        private void ProcessesClick(object Sender, MouseEventArgs Arguments)
        {
            if (LbProcesses.SelectedItem == null)
                return;
            var Item = LbProcesses.SelectedItem as ProcessItem;
            TbProcess.Text = Item.Text;
        }

        private void DLLsClick(object Sender, MouseEventArgs Arguments)
        {
            if (LbDLLs.SelectedItem == null)
                return;
            var Item = LbDLLs.SelectedItem;
            TbDLL.Text = LbDLLs.GetItemText(Item);
        }

    }

}
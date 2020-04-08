using WxInjector.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace WxInjector.Graphics
{

    [SuppressMessage("Globalization", "CA1303")]
    public partial class WnMain : Form
    {

        private Data User = null;

        public WnMain()
        {
            InitializeComponent();
        }

        [SuppressMessage("Design", "CA1031")]
        private void Inject(object Sender, EventArgs Arguments)
        {
            var Item = LbProcesses.SelectedItem as Item;
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
            var Dialog = new OpenFileDialog();
            Dialog.Filter = "Dynamic Link Library|*.dll";
            if (Dialog.ShowDialog() == DialogResult.OK)
                LbDLLs.Items.Add(Dialog.FileName);
            Dialog.Dispose();
        }

        private void Remove(object Sender, EventArgs Arguments)
        {
            if (LbDLLs.SelectedItem != null)
                LbDLLs.Items.Remove(LbDLLs.SelectedItem);
        }

        private void Clear(object Sender, EventArgs Arguments)
        {
            LbDLLs.Items.Clear();
        }

        private void Refresh(object Sender, EventArgs Arguments)
        {
            LbProcesses.Items.Clear();
            var Processes = Process.GetProcesses();
            foreach (var Process in Processes)
            {
                if (!string.IsNullOrEmpty(Process.MainWindowTitle))
                    LbProcesses.Items.Add(new Item(Process.ProcessName + ".exe", Process.Id));
                Process.Dispose();
            }
        }

        private void Start(object Sender, EventArgs Arguments)
        {
            User = Data.Load();
            if (User.DLLs != null)
                foreach (var Item in User.DLLs)
                    LbDLLs.Items.Add(Item);
            Refresher.Start();
        }

        private void Release(object Sender, FormClosingEventArgs Arguments)
        {
            Refresher.Stop();
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

        private class Item
        {

            public int Process = -1;
            public string Text = null;

            public Item(string Text, int Process)
            {
                this.Text = Text;
                this.Process = Process;
            }

            public override string ToString()
            {
                return Text;
            }

        }

        [Serializable]
        private class Data
        {

            public static string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WxInjector.cfg");
            private static BinaryFormatter Formatter = new BinaryFormatter();

            public string[] DLLs = null;

            public void Save()
            {
                using (var Stream = new FileStream(Source, FileMode.Create))
                    Formatter.Serialize(Stream, this);
            }

            public static Data Load()
            {
                if (File.Exists(Source))
                    using (var Stream = new FileStream(Source, FileMode.Open))
                        return Formatter.Deserialize(Stream) as Data;
                return new Data();
            }

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

    }

}
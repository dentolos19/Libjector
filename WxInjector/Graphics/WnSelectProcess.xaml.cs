using System.Diagnostics;
using System.Windows;
using WxInjector.Core.Bindings;

namespace WxInjector.Graphics
{

    public partial class WnSelectProcess
    {

        public int SelectedProcessId { get; private set; }
        public string SelectedProcessName { get; private set; }

        public WnSelectProcess()
        {
            InitializeComponent();
            Refresh(null, null);
        }

        private void Select(object sender, RoutedEventArgs args)
        {
            var item = (ProcessItemBinding)ProcessList.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Select a running process before continuing!", "WxInjector");
                return;
            }
            SelectedProcessId = item.Id;
            SelectedProcessName = item.Name;
            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void Refresh(object sender, RoutedEventArgs args)
        {
            ProcessList.Items.Clear();
            foreach (var process in Process.GetProcesses())
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle))
                    continue;
                ProcessList.Items.Add(ProcessItemBinding.Create(process));
            }
        }

    }

}
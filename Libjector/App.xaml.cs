using System.Windows;

namespace Libjector;

public partial class App
{

    private void OnStartup(object sender, StartupEventArgs args)
    {
        new MainWindow().Show();
    }

}
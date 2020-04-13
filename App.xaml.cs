using System.Windows;
using WxInjector.Graphics;

namespace WxInjector
{

    public partial class App
    {

        private void Initialize(object sender, StartupEventArgs e)
        {
            new WnMain().Show();
        }

    }

}
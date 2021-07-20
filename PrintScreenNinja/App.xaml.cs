using System;
using System.Windows;


namespace PrintScreenNinja
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Properties.Add(ApplicationPropertyKeys.BitmapPersistent, new Core.BitmapPersistent());
        }
    }
}

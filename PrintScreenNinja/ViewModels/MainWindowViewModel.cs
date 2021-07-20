using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows;
using Reactive.Bindings.Extensions;
using PrintScreenNinja.Core;


namespace PrintScreenNinja.ViewModels
{
    sealed class MainWindowViewModel : ViewModelBase
    {
        private Window? _mainWindow;

        public MainWindowViewModel()
        {
            var app = Application.Current;
            var bitmapPersistent = app.GetProperty<BitmapPersistent>(ApplicationPropertyKeys.BitmapPersistent);

            ClipboardSniffer.BitmapBytesChanged
                .SelectMany(xs => bitmapPersistent.SaveAsync(xs).ToObservable())
                .Subscribe()
                .AddTo(this.CompositeDisposable);
        }

        public void OnSourceInitialized(object sender, EventArgs e)
        {
            if (sender is FrameworkElement frameworkElement && frameworkElement.TryFindParent<Window>(out var window))
            {
                _mainWindow = window;
            }
        }

        public void OnClosed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void OnConfigClick(object sender, RoutedEventArgs e)
        {
            new Views.Configuration() { Owner = _mainWindow }.ShowDialog();
        }

        public void OnAboutClick(object sender, RoutedEventArgs e)
        {
            new Views.About() { Owner = _mainWindow }.ShowDialog();
        }

        public void OnExitClick(object sender, RoutedEventArgs e)
        {
            this.Dispose();
            Application.Current.Shutdown();
        }
    }
}

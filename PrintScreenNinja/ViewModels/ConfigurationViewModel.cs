using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using PrintScreenNinja.Core;


namespace PrintScreenNinja.ViewModels
{
    sealed class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel()
        {
            var app = Application.Current;
            var bitmapPersistent = app.GetProperty<BitmapPersistent>(ApplicationPropertyKeys.BitmapPersistent);

            this.SaveDirectory = new ReactivePropertySlim<string>(bitmapPersistent.Directory)
                .AddTo(this.CompositeDisposable);

            this.SaveDirectory.Subscribe(this.OnSaveDirectoryEdited)
                .AddTo(this.CompositeDisposable);
        }

        public ReactivePropertySlim<string> SaveDirectory { get; }

        public void OnContentRendered(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                var textBox = window.EnumerateVisuals().OfType<TextBox>().FirstOrDefault();
                if (textBox is not null)
                {
                    textBox.Focus();
                    textBox.CaretIndex = textBox.Text.Length;
                }
            }
        }

        private void OnSaveDirectoryEdited(string value)
        {
            // validation?
        }

        public void OnOkeyClick(object sender, RoutedEventArgs e)
        {
            var saveDirectory = this.SaveDirectory.Value;

            var app = Application.Current;
            var bitmapPersistent = app.GetProperty<BitmapPersistent>(ApplicationPropertyKeys.BitmapPersistent);

            bitmapPersistent.SetDirectory(saveDirectory);

            if (sender is FrameworkElement frameworkElement && frameworkElement.TryFindParent<Window>(out var window))
            {
                window.Close();
            }
        }
    }
}

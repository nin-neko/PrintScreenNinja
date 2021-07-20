using System;
using System.Threading;
using System.ComponentModel;
using System.Reactive.Disposables;


namespace PrintScreenNinja.ViewModels
{
    abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase() { }

        protected CompositeDisposable CompositeDisposable { get; } = new();

        public void Dispose() => this.CompositeDisposable.Dispose();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
            => Interlocked.CompareExchange(ref this.PropertyChanged, null, null)?.Invoke(this, e);
    }
}

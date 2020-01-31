using BasicMvvm;
using BasicMvvm.Commands;
using oClock.Shared.Helpers;
using System;
using Windows.UI.Xaml.Controls;

namespace oClock.Shared.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel()
        {
            BackCommand = new DelegateCommand<object>(OnBackCommandExecute);
        }
        private TimeSpan? _maxTime;

        public TimeSpan? MaxTime
        {
            get { return _maxTime; }
            set
            {
                _maxTime = value;
                if (value != null)
                {
                    LocalSettingsHelper.MarkContainer(SettingContainer.MaxTime, nameof(SettingContainer.MaxTime), value);
                }
                OnPropertyChanged();
            }
        }
        public DelegateCommand<object> BackCommand { get; }

        private void OnBackCommandExecute(object obj)
        {
            var page = obj as Page;
            if (page != null)
            {
                if (page.Frame.CanGoBack)
                    page.Frame.GoBack();
            }
        }
    }
}

using BasicMvvm;
using BasicMvvm.Commands;
using Windows.UI.Xaml.Controls;

namespace oClock.Shared.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel()
        {
            BackCommand = new DelegateCommand<object>(OnBackCommandExecute);
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

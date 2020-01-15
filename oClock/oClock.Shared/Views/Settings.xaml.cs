using oClock.Shared.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace oClock.Shared.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public SettingsViewModel ViewModel { get; } = new SettingsViewModel();
        public Settings()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}

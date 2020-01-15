using BasicMvvm;
using BasicMvvm.Commands;
using oClock.Shared.Helpers;
using oClock.Shared.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace oClock.Shared.ViewModels
{

    public class ClockViewModel : BindableBase
    {
        readonly DispatcherTimer Timer = new DispatcherTimer();

        public ClockViewModel()
        {

            RemainingTime = "--:--:--";
            IsTimePickerVisible = Visibility.Collapsed;
            IsButtonVisible = Visibility.Visible;
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            CurrentCheckInTimeCommand = new AsyncCommand(OnCurrentCheckInTimeCommandExecutedAsync);
            InputCheckInTimeCommand = new AsyncCommand(OnInputCheckInTimeCommandExecutedAsync);
            SettingCommand = new DelegateCommand<object>(OnSettingCommandExecute);
            //#if NETFX_CORE
            _ = SetTimerAsync();
            //#endif
        }

        private void OnSettingCommandExecute(object obj)
        {
            var page = obj as Page;
            if (page != null)
            {
                page.Frame.Navigate(typeof(Settings));
            }
        }

        private async Task SetTimerAsync()
        {
            var todayTime = await GetTodayCheckInTimeAsync();
            if (!string.IsNullOrEmpty(todayTime))
            {
                TodayCheckInTime = TimeSpan.Parse(todayTime);
            }
        }

        private async Task OnInputCheckInTimeCommandExecutedAsync()
        {

            //#if NETFX_CORE
            if (await CheckInExistAsync())
            {
                IsTimePickerVisible = Visibility.Visible;
                IsButtonVisible = Visibility.Collapsed;
            }
            //#else
            //                IsTimePickerVisible = Visibility.Visible;
            //                IsButtonVisible = Visibility.Collapsed;

            //#endif
        }

        private async Task OnCurrentCheckInTimeCommandExecutedAsync()
        {
            //#if NETFX_CORE
            if (await CheckInExistAsync())
            {
                TodayCheckInTime = DateTime.Now.TimeOfDay;
            }
            //#else
            //            TodayCheckInTime = DateTime.Now.TimeOfDay;
            //#endif
        }
        private async Task<bool> CheckInExistAsync()
        {
            string toDayTime = await GetTodayCheckInTimeAsync();
            if (!string.IsNullOrEmpty(toDayTime))
            {
                var response = await DialogHelper.ConfirmAsync("Today's Checkin Time already exist, Do you want to update?", "oClock", DialogButtons.YesNo);
                if (response == DialogResults.No)
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<string> GetTodayCheckInTimeAsync()
        {
            try
            {
                return LocalSettingsHelper.GetContainerValue<string>(SettingContainer.CheckInTime, DateTime.Now.Date.ToString());
            }
            catch (Exception ex)
            {
                await DialogHelper.AlertAsync(ex.Message);
                return string.Empty;
            }

        }
        private async Task SetTodayCheckInTimeAsync(TimeSpan? value)
        {
            try
            {
                LocalSettingsHelper.MarkContainer(SettingContainer.CheckInTime, DateTime.Now.Date.ToString(), value);
            }
            catch (Exception ex)
            {
                await DialogHelper.AlertAsync(ex.Message);
            }
        }

        public ICommand CurrentCheckInTimeCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand InputCheckInTimeCommand { get; set; }
        private string _remainingTime;
        private Visibility _isButtonVisible;
        private Visibility _isTimePickerVisible;
        private TimeSpan? _todayCheckIntime;
        private string _currentTime;

        public TimeSpan MaxHrs { get; set; } = new TimeSpan(9, 0, 0);
        public Visibility IsButtonVisible
        {
            get { return _isButtonVisible; }
            set
            {
                Set(ref _isButtonVisible, value);
            }
        }
        public Visibility IsTimePickerVisible
        {
            get { return _isTimePickerVisible; }
            set
            {
                Set(ref _isTimePickerVisible, value);
            }
        }
        public string RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                Set(ref _remainingTime, value);
            }
        }

        public TimeSpan? TodayCheckInTime
        {
            get { return _todayCheckIntime; }
            set
            {
                Set(ref _todayCheckIntime, value);
                if (value != null)
                {
                    IsTimePickerVisible = Visibility.Collapsed;
                    IsButtonVisible = Visibility.Visible;
                    //#if NETFX_CORE
                    _ = SetTodayCheckInTimeAsync(value);
                    //#endif
                }

            }
        }



        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                Set(ref _currentTime, value);
            }
        }
        private void Timer_Tick(object sender, object e)
        {
            CurrentTime = DateTime.Now.ToString("hh:mm:ss");
            if (TodayCheckInTime == null)
            {
                RemainingTime = "--:--:--";
                return;
            }
            RemainingTime = MaxHrs.Subtract(DateTime.Now.TimeOfDay.Subtract(TodayCheckInTime.Value)).ToString(@"hh\:mm\:ss");


        }
    }
}

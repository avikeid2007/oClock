using oClock.Shared.Core;
using oClock.Shared.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace oClock.Shared.ViewModels
{
    public class ClockViewModel : Observable
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
#if NETFX_CORE
            var todayTime = GetTodayCheckInTime();
            if (!string.IsNullOrEmpty(todayTime))
            {
                TodayCheckInTime = TimeSpan.Parse(todayTime);
            }
#endif
        }

        private async Task OnInputCheckInTimeCommandExecutedAsync()
        {

#if NETFX_CORE
            if (await CheckInExistAsync())
            {
                IsTimePickerVisible = Visibility.Visible;
                IsButtonVisible = Visibility.Collapsed;
            }
#endif
        }

        private async Task OnCurrentCheckInTimeCommandExecutedAsync()
        {
#if NETFX_CORE
            if (await CheckInExistAsync())
            {
                TodayCheckInTime = DateTime.Now.TimeOfDay;
            }
#else
            TodayCheckInTime = DateTime.Now.TimeOfDay;
#endif
        }
        private async Task<bool> CheckInExistAsync()
        {
            string toDayTime = GetTodayCheckInTime();
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

        private static string GetTodayCheckInTime()
        {
            return LocalSettingsHelper.GetContainerValue<string>(SettingContainer.CheckInTime, DateTime.Now.Date.ToString());
        }

        public ICommand CurrentCheckInTimeCommand { get; set; }
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
#if NETFX_CORE
                    LocalSettingsHelper.MarkContainer(SettingContainer.CheckInTime, DateTime.Now.Date.ToString(), value);
#endif
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

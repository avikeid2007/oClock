using oClock.Shared.Core;
using System;
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
            CurrentCheckInTimeCommand = new RelayCommand(OnCurrentCheckInTimeCommandExecuted);
            InputCheckInTimeCommand = new RelayCommand(OnInputCheckInTimeCommandExecuted);
        }

        private void OnInputCheckInTimeCommandExecuted()
        {
            IsTimePickerVisible = Visibility.Visible;
            IsButtonVisible = Visibility.Collapsed;
            //TimePicker arrivalTimePicker = new TimePicker();
            //var time = arrivalTimePicker.Time;
        }

        private void OnCurrentCheckInTimeCommandExecuted()
        {
            TodayCheckInTime = DateTime.Now.TimeOfDay;


        }

        public RelayCommand CurrentCheckInTimeCommand { get; set; }
        public RelayCommand InputCheckInTimeCommand { get; set; }
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

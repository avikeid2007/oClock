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
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            CurrentCheckInTimeCommand = new RelayCommand(OnCurrentCheckInTimeCommandExecuted);
            InputCheckInTimeCommand = new RelayCommand(OnInputCheckInTimeCommandExecuted);
        }

        private void OnInputCheckInTimeCommandExecuted()
        {

        }

        private void OnCurrentCheckInTimeCommandExecuted()
        {
            TodayCheckInTime = DateTime.Now;


        }

        public RelayCommand CurrentCheckInTimeCommand { get; set; }
        public RelayCommand InputCheckInTimeCommand { get; set; }
        private string _remainingTime;

        public string RemainingTime
        {
            get { return _remainingTime; }
            set
            {

                Set(ref _remainingTime, value);
            }
        }
        public TimeSpan MaxHrs { get; set; } = new TimeSpan(9, 0, 0);
        private DateTime? _todayCheckIntime;
        public DateTime? TodayCheckInTime
        {
            get { return _todayCheckIntime; }
            set
            {
                Set(ref _todayCheckIntime, value);
            }
        }

        private string _currentTime;
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
            var time = DateTime.Now.Subtract(TodayCheckInTime.Value);
            var rem = MaxHrs.Subtract(time);
            RemainingTime = rem.ToString();

        }
    }
}

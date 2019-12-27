using oClock.Shared.Core;
using System;
using Windows.UI.Xaml;

namespace oClock.Shared.ViewModels
{
    public class ClockViewModel : Observable
    {
        readonly DispatcherTimer Timer;
        public ClockViewModel()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            CurrentTime = DateTime.Now.ToString("hh:mm:ss");
        }
    }
}

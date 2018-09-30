using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.Devices.Gpio;

namespace Watchdog
{
    class WatchdogResetter : IDisposable
    {
        private const int PET_INTERVAL_IN_SECONDS = 10;
        private const int WATCHDOG_PET_PIN = 18;
        private GpioPin m_WatchdogPetPin;
        private Timer m_Timer;
        public WatchdogResetter()
        {
            m_WatchdogPetPin = GpioController.GetDefault().OpenPin(WATCHDOG_PET_PIN);
            m_Timer = new Timer(TimeSpan.FromSeconds(PET_INTERVAL_IN_SECONDS).TotalMilliseconds);
            m_Timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            m_Timer.Start();
        }

        public void Stop()
        {
            m_Timer.Stop();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await PetTheDog();
        }


        private async Task PetTheDog()
        {
            m_WatchdogPetPin.SetDriveMode(GpioPinDriveMode.Output);
            Debug.WriteLine($"{DateTime.Now} PIN {m_WatchdogPetPin.PinNumber} set as {m_WatchdogPetPin.GetDriveMode()}");

            m_WatchdogPetPin.Write(GpioPinValue.Low);
            Debug.WriteLine($"{DateTime.Now} PIN {m_WatchdogPetPin.PinNumber} set LOW");
            await Task.Delay(500);

            //m_WatchdogPetPin.Write(GpioPinValue.High);
            //Debug.WriteLine($"{DateTime.Now} PIN {m_WatchdogPetPin.PinNumber} set HIGH");
            m_WatchdogPetPin.SetDriveMode(GpioPinDriveMode.Input);
            Debug.WriteLine($"{DateTime.Now} PIN {m_WatchdogPetPin.PinNumber} set {m_WatchdogPetPin.GetDriveMode()} ");

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Watchdog() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

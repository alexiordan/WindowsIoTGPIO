using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using System.Threading;
using System.Diagnostics;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Watchdog
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const int PAT_INTERVAL_IN_MS = 10000;
        private const int SIGNAL_DURATION_IN_MS = 500;
        private const int WATCHDOG_PIN = 18;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            var watchdogPin = GpioController.GetDefault().OpenPin(WATCHDOG_PIN);
            while (true)
            {
                watchdogPin.SetDriveMode(GpioPinDriveMode.Output);
                Debug.WriteLine($"{DateTime.Now} PIN {watchdogPin.PinNumber} set as {watchdogPin.GetDriveMode()}");

                watchdogPin.Write(GpioPinValue.Low);
                Debug.WriteLine($"{DateTime.Now} PIN {watchdogPin.PinNumber} set LOW");
                Thread.Sleep(SIGNAL_DURATION_IN_MS);

                //m_WatchdogPetPin.Write(GpioPinValue.High);
                //Debug.WriteLine($"{DateTime.Now} PIN {m_WatchdogPetPin.PinNumber} set HIGH");
                watchdogPin.SetDriveMode(GpioPinDriveMode.Input);
                Debug.WriteLine($"{DateTime.Now} PIN {watchdogPin.PinNumber} set {watchdogPin.GetDriveMode()} ");

                Thread.Sleep(PAT_INTERVAL_IN_MS);
            }
        }
    }
}

#
# Test WatchDog Blink
# From here: http://forum.switchdoc.com/thread/444/watchdog-reboots-system-4-seconds#ixzz5SVUJ14YN

# SwitchDoc Labs July 2017

# Note: With programs like this, you may have to exit by using CTRL+|
#

# Wire up power to pins on WatchDog (Note: We are NOT using the Grove Connector in this case)
#
# Note: Pin 1 and Pin 2 of the Grove Connector needs to be connected to VDD for the pin inputs to work correctly.

import RPi.GPIO as GPIO
import time

GPIO.setmode(GPIO.BCM)

# Patting line
RESET_WATCHDOG1 = 18

def resetWatchDog():

        GPIO.setup(RESET_WATCHDOG1, GPIO.OUT)
        GPIO.output( RESET_WATCHDOG1, False)
        time.sleep(0.500)
        GPIO.setup(RESET_WATCHDOG1, GPIO.IN)


try:
        while True:
                print "patting"
                resetWatchDog();
                time.sleep(10)
# I have tried also with the to GPIO.cleanup() lines replaced by dummy prints. And closing the app durin pin was set as input.
except KeyboardInterrupt:
        GPIO.cleanup() # clean up GPIO on CTRL+C exit
GPIO.cleanup() # clean up GPIO on normal exit

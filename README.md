# KeyboardToControllerMapper
Allows mapping a virtual controller button press to the press of a keyboard key, like a reverse Joy2Key.

# KeyboardToControllerMapper Usage
Controller number is the currently selected controller.
The program automatically plugs in controller 1 when it starts, and unplugs all when it closes cleanly.
You can unplug 1 and plug in a different controller number if you wish.

Load/save - Loads and saves the mapped keys to "settings.txt".
This is automatically loaded on program start.

The rest is mainly selecting and showing the desired mappings.
Use "Find By Key Press"! Click it then press the desired key on your keyboard.
The official names can be a bit confusing.

# Driver Install Instructions
Firstly, save your work, this has been known to crash stuff, as in Windows itself.

IMPORTANT:
As this program mimics an xbox 360 controller we need to have xbox 360 drivers installed.
So make sure xbox 360 controller drivers are installed.
On Win 8/10 this may not be necessary as they apparently come by default, but on Win 7 make sure they are installed.

Secondly, the driver is by Scarlet.Crush and orginally for getting PS3/PS4 controllers to work on windows.
So, if you are using another program for a similar thing there may a possible conflict as this has not been tested at all.
However, this driver should just be a virtual xbox 360 emulation driver.
Or, if you are already using ScpToolkit for this, then this driver install may not be required, as you may already have a compatible driver installed (try opening KeyboardToControllerMapper and see if it tells you to install a driver).

Now install the driver. This requires privileges as it is a driver install. It may automatically prompt for this (it has for me), otherwise make sure it has sufficient privileges to install the driver.

If the KeyboardToControllerMapper software says to install the driver, just install the driver again I guess.
We have encountered it where it needs to be reinstalled before every use for some odd reason.

# Copyright
Based on code from Morgan Zolob (Mogzol), his code Licensed under MIT. 
  github.com/mogzol/ScpDriverInterface
His code, and the whole of the driver, is based off code from Scarlet.Crush, see forum thread.
  http://forums.pcsx2.net/Thread-XInput-Wrapper-for-DS3-and-Play-com-USB-Dual-DS2-Controller
InterceptKeys.cs is mostly by Microsoft, see the start of the file for details.
Any modifications to the above, or additional files, are copyright Elliot Dawber (d265f27) 2017, MIT License.

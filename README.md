# NowPlayingWidget

This is an overlay for OBS displaying the media currently playing on Windows.

<img src="assets/preview.gif" />

# Using this widget
Add a new browser source in your scene then change these properties : 

	- URL: http://localhost:61001 
	- width: 300
	- height: 90

Note: you can change listening URL and port by editing the appsettings.json file.

# Display options on default template
There are a few parameters to change the display.
	
	- progress : 0/1 to hide/show the progress bar (default is visible)
	- status : 0/1 to hide/show the animated equalizer (default is visible)
	- rgb : 0/1 to disable/enable colors for the animated equalizer (default is off)

#### Example:
Hide the progress bar and enable colors for the animated equalizer
```
http://localhost:61001?progress=0&rgb=1
```
# DiskUnlocker

Has this ever happened to you?

![This device is currently in use. Close any programs or windows that might be using the device and then try again.](https://raw.github.com/K-Wojciechowski/DiskUnlocker/main/screenshots/error.png)

Sometimes, the culprit is obvious. Sometimes, closing Explorer is enough. However, there might be background processes (like WSL or SearchIndexer) holding onto file handles and blocking the device removal.

Windows does not display the offending process in the UI, but the Event Log can reveal it.

This is where DiskUnlocker comes in: it reads the event log and displays a list of recent failures. It also offers a Kill button to get rid of the offending process.

![DiskUnlocker UI](https://raw.github.com/K-Wojciechowski/DiskUnlocker/main/screenshots/DiskUnlocker.png)

Note the list usually includes a `System` entry. If `System` is accompanied by another entry pointing at another process, it is likely killing this process will unblock the `System` process too. If `System` is the only entry, you may need to look for issues on your own.

## License

The code is MIT-licensed. The icon is taken from `shell32.dll`.

This software is provided AS-IS. I take no responsibility if something breaks, in particular if an important process is killed.

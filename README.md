# ClearRecentLinks
Clear specific Recent/QuickAccess links in Windows 10

Problem: You can enable or disable Windows 10 quickaccess globally,
but you cannot exclude specific folders or files.

Solution: this app

This application runs every n minutes (default 15) and process two folders:
1) shortcuts in c:\users\USER\AppData\Roaming\Microsoft\Windows\Recent
2) "jump list" files (*.automaticDestinations-ms) in c:\users\USER\AppData\Roaming\Microsoft\Windows\Recent\AutomaticDestinations

You supply a list of things you don't want to show up in Recent / QuickAccess, and this app will delete the links to them.
You can specify any or all of these things:
a) directory, e.g.  c:\pr0n
b) full filename, e.g. my-secrets.txt
c) partial filename, e.g.  secret 


Future: I will add a try icon, here's some info
https://www.c-sharpcorner.com/UploadFile/f9f215/how-to-minimize-your-application-to-system-tray-in-C-Sharp/

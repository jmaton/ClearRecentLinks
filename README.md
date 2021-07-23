# ClearRecentLinks - 
A small C# app to clear specific Recent/QuickAccess links in Windows 10

## Problem: You can enable or disable Windows 10 QuickAccess globally, but you cannot exclude specific folders or files.

## Solution: this app

This application runs every n minutes (default 15) and processes two folders:
1. shortcuts in c:\users\USER\AppData\Roaming\Microsoft\Windows\Recent
2. "jump list" files (*.automaticDestinations-ms) in c:\users\USER\AppData\Roaming\Microsoft\Windows\Recent\AutomaticDestinations

You supply a list of things you don't want to show up in Recent / QuickAccess, and this app will delete the links to them.
You can specify any or all of these things:
- directory, e.g.  c:\pr0n
- full filename, e.g. my-secrets.txt
- partial filename, e.g.  secret 


Future: I will add a try icon, here's some info for my use:
https://www.c-sharpcorner.com/UploadFile/f9f215/how-to-minimize-your-application-to-system-tray-in-C-Sharp/

# SharpShares
Quick domain share enumeration tool


Compile, execute from within a domain joined machine, it collates a list of all domain joined hosts, and then attempts to enumerate any exposed shares that your account can access.

Bish Bash Bosh

```
PS C:\Users\g.white> C:\Users\g.white\Desktop\SharpShareRelease2.exe
\\WIN-MS87LHLC91U\NETLOGON
\\WIN-MS87LHLC91U\SYSVOL
\\LABLAB-PC1\The-Shares
\\LABLAB-PC1\Users
PS C:\Users\g.white> 
```



The .sln file should do everything for you, but just in case I made some notes below.

Before you try and compile it check that you have installed System.DirectoryServices 7.0.1 via NuGet then it should compile.
It should work with future updates of System.DirectoryServices I just picked 7.0.1 because it was the latest at the time of creation.

Compile instructions, double click the SharpShareRelease2.sln file, this will open Visual Studio (VS), I’m using VS 2022.

On the right under Solution Explorer, right click on SharpShareRelease2 and select ‘Manage NuGet Packages …’ This will open the NuGet Tab back on the left side of the screen, click on Installed and you should see System.DirectoryServices if you don’t click on the Browse tab next to Installed and search for System.DirectoryServices and install it. 7.0.1 was latest at time of creation.

Then try and run it, if you get the exe pop up, its looking good, to publish go back to Solution Explorer, right click on SharpShareRelease2 and select Publish, click on Show all Settings, and select 

```Configuration: Release | Any CPU
Target framework: net6.0
Deployment mode: Self-contained
Target runtime: win-x86
Target location: set to what you want.
```

Then click on the File publish option arrow which is just under Target location, and tick Produce single file. Click save then select Publish towards the top middle of the page.

This should then publish the .exe

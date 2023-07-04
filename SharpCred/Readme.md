# SharpCred


SharpCred allows users to authenticate with a domain, generate or load a list of usernames, and check if a provided password matches any of the usernames. It also retrieves the domain groups associated with valid credentials and identifies high privileged accounts based on predefined groups. The program can enumerate the domain password policy and provides an interactive menu for performing these operations.


To compile double click the ShapCred_Release.sln file which should open Visual Studio for you.

<INSERT IMAGE>
 
If you want to look at the C# click on Program.cs under the Solution Explorer

<INSERT IMAGE>


The file contains all that is required to compile so you can just right click on SharpCred_Release and select Publish.

<INSERT IMAGE>


If you click on Show all settings you can see the publish settings, which will compile SharpCred as self-contained single binary file.



<INSERT IMAGE>

Once you have published the file you can then copy the binary to a testing machine.


<INSERT IMAGE>
 
The program can be run from domain or nondomain machines, to connect from a nondomain machine, verify that you can ping the full domain to confirm DNS routing.


<INSERT IMAGE>

 
If you can’t ping the full domain, check the DNS setting on your test machine which should point to the domain DNS IP address, which is typically the domain controller’s IP address.
In the lab we use DHCP for the VM’s but add a static IP address for local DNS pointing at the lab domain controllers IP address of 192.168.68.220.



<INSERT IMAGE>
 

To execute it you can either double click it or open it in CMD/PowerShell.



<INSERT IMAGE>

 
You are then asked to authenticate with the domain. If you add a wrong credential, it will be rejected by the local domain controller, and you are prompted to try again. You can authenticate with an account belonging to only the domain users’ group.



<INSERT IMAGE>
 

Successful authentication will result in complete menu loading. All options are executed over LDAP, and it is advised to execute option 3 first, which will enumerate the domain password and lockout policy.



<INSERT IMAGE>

 
A lockout threshold of 0 is the default setting in a domain, it means no policy has been set and you can attempt as many passwords as you wish without risk of lockouts.



<INSERT IMAGE>

 
If you see any number other than 0 take note of it, as that is the amount of times you could attempt password choices within the defined time period, to be carful it would be recommended to not attempt 2 less tries than the number specifies, example if the Lockout Threshold was set to 5 it would be advised to only attempt 3 tries within a time period.

You can return to the original menu by pressing b.

```
Enter a password to try against the usernames ('b' to return to the original menu or 'q' to quit): b
Choose an option:
1. Generate usernames using LDAP query
2. Provide usernames from a file
3. Enumerate domain password policy
4. Quit

Enter your choice:

```

Option 1: Scrapes a list of all of domain usernames using LDAP.



<INSERT IMAGE>

 
It lists the number of usernames that are harvested and then offers you the option to save a copy to disk if you wish to. These usernames are stored in memory and are used as the target list during the password stuffing attempt.


Saving the harvested username list back to disk.



<INSERT IMAGE>

 
After pressing enter you are asked to add a password choice, be careful as each attempt will be executed so check for typos, when ready press enter.



<INSERT IMAGE>


 
The program then will attempt to authenticate with the domain controller using the harvested username list combined with the password choice. Any successful matches are printed to the console, followed by their domain group, and any accounts belonging to a high privileged group.



<INSERT IMAGE>


 
Option 2: Allows you to use your own list of usernames, which can be used as a stealthy approach when in a security mature environment.



<INSERT IMAGE>


Usernames should be provided in a list with one username per line.


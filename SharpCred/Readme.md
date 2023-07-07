# SharpCred (Offensive SysAdmin Suite)


SharpCred allows users to authenticate with a domain, generate or load a list of usernames, and check if a provided password matches any of the usernames. It also retrieves the domain groups associated with valid credentials and identifies high privileged accounts based on predefined groups. The program can enumerate the domain password policy and provides an interactive menu for performing these operations.


To compile double click the ShapCred_Release.sln file which should open Visual Studio for you.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/252389af-694b-41e0-867f-51ece34b8628)

 
If you want to look at the C# click on Program.cs under the Solution Explorer

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/14b44e04-4681-4c34-8bd4-45de80161db3)



The file contains all that is required to compile so you can just right-click on SharpCred_Release and select Publish.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/be04610c-73b3-4af6-9dc1-bac2564570b5)


If you click on Show all settings, you can see the publish settings, which will compile SharpCred as self-contained single binary file.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/627d7dac-8411-4699-a778-a23c3da3d46e)


Once you have published the file, you can then copy the binary to a testing machine.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/5dfeb63a-5ad9-471f-acee-2c9d120e06cc)
 
The program can be run from domain or nondomain machines, to connect from a nondomain machine, verify that you can ping the full domain to confirm DNS routing.


![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/bd728f72-5b86-44e2-b76f-59b8ecbb9c8c)

 
If you can’t ping the full domain, check the DNS setting on your test machine, which should point to the domain DNS IP address, which is typically the domain controller’s IP address.
In the lab we use DHCP for the VMs but add a static IP address for local DNS pointing at the lab domain controllers IP address of 192.168.68.220.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/7261a590-49b1-4b65-8fbd-a61e19515299)


To execute it, you can either double click it or open it in CMD/PowerShell.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/8b0acac5-9e6f-4205-a03c-1adcc62360f7)


 
You are then asked to authenticate with the domain. If you add a wrong credential, it will be rejected by the local domain controller, and you are prompted to try again. You can authenticate with an account belonging to only the domain users’ group.


![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/d7a50ce6-3739-4c2a-b72f-88edcf01e9f2)

 

Successful authentication will result in complete menu loading. All options are executed over LDAP, and it is advised to execute option 3 first, which will enumerate the domain password and lockout policy.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/f2d6c764-1229-43d5-98be-39063f33c4c3)

 
A lockout threshold of 0 is the default setting in a domain, it means no policy has been set and you can attempt as many passwords as you wish without risk of lockouts.

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/0b932d0c-4382-4791-835a-fe7b400f12fc)


 
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

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/dac1e5c2-9553-42b8-890b-3c250f1b7fde)

 
It lists the number of usernames that are harvested and then offers you the option to save a copy to disk if you wish to. These usernames are stored in memory and are used as the target list during the password stuffing attempt.


Saving the harvested username list back to disk.


![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/203605bf-25eb-4761-8ced-45259a4b983f)


 
After pressing enter you are asked to add a password choice, be careful as each attempt will be executed so check for typos, when ready press enter.


![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/c106c939-5ee1-4059-9620-d39547f91823)



 
The program then will attempt to authenticate with the domain controller using the harvested username list combined with the password choice. Any successful matches are printed to the console, followed by their domain group, and any accounts belonging to a high privileged group.


![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/79de2c47-a737-4c55-af64-45eb397efae2)


 
Option 2: Allows you to use your own list of usernames, which can be used as a stealthy approach when in a security mature environment.


![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/c9e9d63e-2fc1-47e1-add7-7ea046931ad2)



Usernames should be provided in a list with one username per line.


# Offensive Sysadmin aka Adversary Kit 
A collection of tools demonstrated at our recent talk, Adversaries Have It Easy, brought to you by [Neil Lines](https://twitter.com/myexploit2600) & [Andy Gill](https://twitter.com/ZephrFish) at [Lares Labs](https://labs.lares.com).

![image](https://github.com/LaresLLC/OffensiveSysAdmin/assets/5783068/7784b1c6-28dd-442e-a4a9-ce3fd77798f2)

The tooling is written in PS and C# using .net 6 for CS binaries. None are provided pre-compiled but instructions on how to do so can be found in the blog post:

https://labs.lares.com/offensive-sysadmin/

## Setup
To pull down all of the tools simply issue:
```
git clone --recurse-submodules -j8 git://github.com/LaresLLC/OffensiveSysAdmin.git
```

Each module has its own readme and can run independently of the suite.

## Tooling
The table below details what each tool does, and the subsections detail how to use each.

| **Name** | **Language** | **Description** |
|--------------|--------------|--------------|
| DomainScrape | PS | Hunt for keywords in documents across domain shares. |
| Invoke-Ghost | PS | Only scrapes metadata from office documents from an entire directory, a stealthy way to grab usernames. |
| [ScrapingKit](https://github.com/LaresLLC/ScrapingKit) | PS & C# | Scraping Kit comprises several tools for scraping services for keywords, useful for initial enumeration of Domain Controllers or if you have popped a user's desktop, their outlook client. |
| SharpCred | C# | Automates the harvesting of domain user accounts / password stuffing/domain groups, which can be used from domain or nondomain joined hosts. |
| SharpShares | C# | Takes no input, executes, and gives you a list of shares the domain user can access. |
| [SlinkyCat](https://github.com/LaresLLC/SlinkyCat) | PS | A collection of AD Audit functions for easy identification of misconfigurations within active directories, users, groups, permissions, and mishandling data within objects |


Read this blog post for more detailed information over on [Lares Labs](https://labs.lares.com/)

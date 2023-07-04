# Offensive Sysadmin aka Adversary Kit
A collection of tools demonstrated at our recent talk, Adversaries Have It Easy, by Neil Lines and Andy Gill.

The table below details what each tool does, and the subsections detail how to use each.

| **Name** | **Language** | **Description** |
|--------------|--------------|--------------|
| DomainScrape | PS | Hunt for keywords in documents across domain shares. |
| Invoke-Ghost | PS | Only scrapes metadata from office documents from an entire directory, a stealthy way to grab usernames. |
| ScrapingKit | PS & C# | Scraping Kit comprises several tools for scraping services for keywords, useful for initial enumeration of Domain Controllers or if you have popped a user's desktop, their outlook client. |
| SharpCred | C# | Automates the harvesting of domain user accounts / password stuffing/domain groups, which can be used from domain or nondomain joined hosts. |
| SharpShares | C# | Takes no input, executes, and gives you a list of shares the domain user can access. |
| SlinkyCat | PS | A collection of AD Audit functions for easy identification of misconfigurations within active directories, users, groups, permissions, and mishandling data within objects |
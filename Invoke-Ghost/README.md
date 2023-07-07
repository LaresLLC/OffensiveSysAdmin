# Invoke-Ghost (Offensive SysAdmin Suite) 
Sneaky sneak way to scrape metadata from office docs using PowerShell from local and remote shares.

``` PS C:\Users\user1\Desktop\Scripts\Ghost> powershell.exe -nop -exec bypass ```


Executing the script on a local machine:

```
PS C:\Users\user1\Desktop\Scripts\Ghost> Import-Module .\Invoke-Ghost.ps1

PS C:\Users\user1\Desktop\Scripts\Ghost> Invoke-Ghost

Enter the directory path: C:\Users\user1\Desktop\Meta2\

Location: C:\Users\user1\Desktop\Meta2
File: fddfdfdf.pptx
Author: Freaky Dico
Created Date: 22/06/2023 12:24
Last Modified Date: 21/06/2023 12:29

Location: C:\Users\user1\Desktop\Meta2
File: Test1.docx
Author: Freaky Dico
Created Date: 22/06/2023 13:23
Last Modified Date: 22/06/2023 13:23

Location: C:\Users\user1\Desktop\Meta2
File: Test2.doc
Author: Freaky Lines
Created Date: 22/06/2023 13:23
Last Modified Date: 22/06/2023 13:23

Location: C:\Users\user1\Desktop\Meta2
File: Test_1.xls
Author: FDico
Created Date: 22/06/2023 12:24
Last Modified Date: 21/06/2023 12:28

Location: C:\Users\user1\Desktop\Meta2
File: Test_2.xlsx
Author: FDico
Created Date: 22/06/2023 12:24
Last Modified Date: 21/06/2023 12:29

```

Executing the script against a domain controllers share path.

```
PS C:\Users\user1\Desktop\Scripts\Ghost> Invoke-Ghost
Enter the directory path: \\hacklab.local\NETLOGON\
Location: \\hacklab.local\NETLOGON
File: fddfdfdf.pptx
Author: Freaky Dico
Created Date: 21/06/2023 12:29
Last Modified Date: 21/06/2023 12:29

Location: \\hacklab.local\NETLOGON
File: Test1.docx
Author: Freaky Dico
Created Date: 22/06/2023 13:23
Last Modified Date: 22/06/2023 13:23

Location: \\hacklab.local\NETLOGON
File: Test2.doc
Author: Freaky Dico
Created Date: 22/06/2023 13:23
Last Modified Date: 22/06/2023 13:23

Location: \\hacklab.local\NETLOGON
File: Test_1.xls
Author: FDico
Created Date: 21/06/2023 12:28
Last Modified Date: 21/06/2023 12:28

Location: \\hacklab.local\NETLOGON
File: Test_2.xlsx
Author: FDico
Created Date: 21/06/2023 12:29
Last Modified Date: 21/06/2023 12:29
```

Executing the script against a remote host share path.

```
PS C:\Users\user1\Desktop\Scripts\Ghost> Invoke-Ghost
Enter the directory path: \\Win11-Host-2\Silly_Share\
Location: \\Win11-Host-2\Silly_Share
File: fddfdfdf.pptx
Author: Freaky Dico
Created Date: 21/06/2023 12:29
Last Modified Date: 21/06/2023 12:29

Location: \\Win11-Host-2\Silly_Share
File: Test1.docx
Author: Freaky Dico
Created Date: 22/06/2023 13:23
Last Modified Date: 22/06/2023 13:23

Location: \\Win11-Host-2\Silly_Share
File: Test2.doc
Author: Freaky Dico
Created Date: 22/06/2023 13:23
Last Modified Date: 22/06/2023 13:23

Location: \\Win11-Host-2\Silly_Share
File: Test_1.xls
Author: FDico
Created Date: 21/06/2023 12:28
Last Modified Date: 21/06/2023 12:28

Location: \\Win11-Host-2\Silly_Share
File: Test_2.xlsx
Author: FDico
Created Date: 21/06/2023 12:29
Last Modified Date: 21/06/2023 12:29

PS C:\Users\user1\Desktop\Scripts\Ghost>

```

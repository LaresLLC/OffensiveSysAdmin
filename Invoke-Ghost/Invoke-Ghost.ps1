function Invoke-Ghost {
    $directoryPath = Read-Host -Prompt "Enter the directory path"
    $shell = New-Object -ComObject Shell.Application

    # Get all files in the directory and its subdirectories
    $files = Get-ChildItem -Path $directoryPath -File -Recurse

    foreach ($file in $files) {
        $folder = Split-Path $file.FullName
        $fileName = $file.Name

        $shellFolder = $shell.Namespace($folder)
        $shellFile = $shellFolder.ParseName($fileName)

        $authorProperty = 20  # Property ID for "Author"
        $createdProperty = 4  # Property ID for "Date created"
        $modifiedProperty = 3  # Property ID for "Date modified"

        $author = $shellFolder.GetDetailsOf($shellFile, $authorProperty)
        $createdDate = $shellFolder.GetDetailsOf($shellFile, $createdProperty)
        $modifiedDate = $shellFolder.GetDetailsOf($shellFile, $modifiedProperty)

        # Only display responses with non-blank Author
        if (![string]::IsNullOrWhiteSpace($author)) {
            # Store the results in a variable with line breaks
            $results = @"
Location: $folder
File: $fileName
Author: $author
Created Date: $createdDate
Last Modified Date: $modifiedDate

"@

            # Output the results
            Write-Host $results
        }
    }
}

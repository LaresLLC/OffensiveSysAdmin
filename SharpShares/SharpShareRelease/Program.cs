using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

public class Program
{
    [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
    public static extern int NetShareEnum(
        string serverName,
        int level,
        ref IntPtr bufPtr,
        int prefMaxLen,
        ref int entriesRead,
        ref int totalEntries,
        ref int resumeHandle
    );

    [DllImport("Netapi32.dll")]
    public static extern int NetApiBufferFree(IntPtr buffer);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHARE_INFO_1
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi1_netname;
        public uint shi1_type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string shi1_remark;
    }

    public static int GetSize()
    {
        return Marshal.SizeOf(typeof(SHARE_INFO_1));
    }

    public static string GDN1()
    {
        string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        if (!string.IsNullOrEmpty(domainName))
        {
            return domainName;
        }
        else
        {
            string hostName = Dns.GetHostName();
            string[] hostNameParts = hostName.Split('.');
            if (hostNameParts.Length > 1)
            {
                return string.Join(".", hostNameParts, 1, hostNameParts.Length - 1);
            }
            else
            {
                return hostName;
            }
        }
    }

    public static void GS1(string[] computerNames)
    {
        foreach (string domainHost in computerNames)
        {
            int queryLevel = 1;
            IntPtr ptrInfo = IntPtr.Zero;
            int entriesRead = 0;
            int totalEntries = 0;
            int resumeHandle = 0;

            int result = NetShareEnum(domainHost, queryLevel, ref ptrInfo, -1, ref entriesRead, ref totalEntries, ref resumeHandle);
            long offset = ptrInfo.ToInt64();

            if (result == 0 && offset > 0)
            {
                int increment = GetSize();

                for (int i = 0; i < entriesRead; i++)
                {
                    IntPtr newIntPtr = new IntPtr(offset);
                    SHARE_INFO_1 info = (SHARE_INFO_1)Marshal.PtrToStructure(newIntPtr, typeof(SHARE_INFO_1));

                    if (info.shi1_netname != "ADMIN$" && info.shi1_netname != "C$" && info.shi1_netname != "IPC$")
                    {
                        Console.WriteLine("\\\\" + domainHost + "\\" + info.shi1_netname);
                    }

                    offset += increment;
                }

                NetApiBufferFree(ptrInfo);
            }
            else if (result != 0)
            {
                // Error occurred, but we skip displaying the error message
                continue;
            }
        }
    }

    public static void Main()
    {
        string domainName = GDN1();
        string ldapPath = "LDAP://" + domainName;

        List<string> computerNames = new List<string>();
        using (System.DirectoryServices.DirectoryEntry root = new System.DirectoryServices.DirectoryEntry(ldapPath))
        {
            using (System.DirectoryServices.DirectorySearcher searcher = new System.DirectoryServices.DirectorySearcher(root, "(objectCategory=computer)"))
            {
                searcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                searcher.PageSize = 1000;
                searcher.PropertiesToLoad.Add("name");

                foreach (System.DirectoryServices.SearchResult result in searcher.FindAll())
                {
                    computerNames.Add(result.Properties["name"][0].ToString());
                }
            }
        }

        GS1(computerNames.ToArray());
    }
}

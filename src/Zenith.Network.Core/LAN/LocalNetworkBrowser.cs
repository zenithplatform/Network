using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Core.LAN
{
    public sealed class LocalNetworkBrowser
    {
        #region Dll Imports

        /// <summary>
        /// Netapi32.dll : The NetServerEnum function lists all servers of the specified type that are
        /// visible in a domain. For example, an application can call NetServerEnum to list all domain controllers only
        /// or all SQL servers only.You can combine bit masks to list several types. For example, a value 
        /// of 0x00000003  combines the bit masks for SV_TYPE_WORKSTATION (0x00000001) and SV_TYPE_SERVER (0x00000002)
        /// </summary>
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetServerEnum(
                            string ServerName, // must be null
                            int dwLevel,
                            ref IntPtr pBuf,
                            int dwPrefMaxLen,
                            out int dwEntriesRead,
                            out int dwTotalEntries,
                            int dwServerType,
                            string domain, // null for login domain
                            out int dwResumeHandle
                        );



        /// <summary>
        /// Netapi32.dll : The NetApiBufferFree function frees  the memory that the NetApiBufferAllocate function allocates. 
        /// Call NetApiBufferFree to free the memory that other network management functions return.
        /// </summary>
        [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        public static extern int NetApiBufferFree(IntPtr pBuf);

        
        [StructLayout(LayoutKind.Sequential)]
        public struct _SERVER_INFO_100
        {
            internal int sv100_platform_id;

            [MarshalAs(UnmanagedType.LPWStr)]
            internal string sv100_name;
        }

        #endregion
        
        public LocalNetworkBrowser()
        {

        }

        /// <summary>
        /// Uses the DllImport : NetServerEnum with all its required parameters
        /// (see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
        /// for full details or method signature) to retrieve a list of domain SV_TYPE_WORKSTATION and SV_TYPE_SERVER PC's
        /// </summary>
        /// <returns>Arraylist that represents all the SV_TYPE_WORKSTATION and SV_TYPE_SERVERPC's in the Domain</returns>
        public List<LocalNetworkMachine> GetLocalNetworkComputers()
        {
            //local fields
            List<LocalNetworkMachine> networkComputers = new List<LocalNetworkMachine>();
            const int MAX_PREFERRED_LENGTH = -1;
            int SV_TYPE_WORKSTATION = 1;
            int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            IntPtr tmpBuffer = IntPtr.Zero;
            int entriesRead = 0;
            int totalEntries = 0;
            int resHandle = 0;
            int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));


            try
            {
                //call the DllImport : NetServerEnum with all its required parameters
                //see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/netserverenum.asp
                //for full details of method signature
                int ret = NetServerEnum(null,
                                        100, 
                                        ref buffer,
                                        MAX_PREFERRED_LENGTH,
                                        out entriesRead,
                                        out totalEntries, 
                                        SV_TYPE_WORKSTATION | SV_TYPE_SERVER, 
                                        null, 
                                        out resHandle);

                if (ret == 0) //NERR_Success
                {
                    //loop through all SV_TYPE_WORKSTATION and SV_TYPE_SERVER PC's
                    for (int i = 0; i < totalEntries; i++)
                    {
                        //get pointer to, Pointer to the buffer that received the data from the call to NetServerEnum. 
                        //Must ensure to use correct size of STRUCTURE to ensure correct location in memory is pointed to
                        tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));

                        //Have now got a pointer to the list of SV_TYPE_WORKSTATION and 
                        //SV_TYPE_SERVER PC's, which is unmanaged memory needs to Marshal data from an unmanaged block of memory to a 
                        //managed object, again using STRUCTURE to ensure the correct data is marshalled 
                        _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));

                        LocalNetworkMachine machine = new LocalNetworkMachine();
                        machine.HostName = svrInfo.sv100_name;                        
                        machine.HostEntry = System.Net.Dns.GetHostEntry(machine.HostName);

                        //add the PC names to the ArrayList
                        networkComputers.Add(machine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //The NetApiBufferFree function frees  the memory that the NetApiBufferAllocate function allocates
                NetApiBufferFree(buffer);
            }

            //return entries found
            return networkComputers;
        }

        public IEnumerable<LocalNetworkMachine> GetLocalNetworkComputers(DiscoveryMethod method)
        {
            return null;
        }
    }

    public struct LocalNetworkMachine
    {
        public string HostName { get; set; }
        public IPHostEntry HostEntry { get; set; }
    }

    public enum DiscoveryMethod
    {
        ActiveDirectory,
        Broadcast
    }
}

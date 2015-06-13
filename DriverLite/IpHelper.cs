using System;
using System.Diagnostics;
using System.Linq;
using Windows.Networking.Connectivity;

namespace DriverLite
{
    public class IpHelper
    {
        public static void GetIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp != null && icp.NetworkAdapter != null)
            {
                var hostname =
                    NetworkInformation.GetHostNames()
                        .SingleOrDefault(
                            hn =>
                            hn.IPInformation != null && hn.IPInformation.NetworkAdapter != null
                            && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

                if (hostname != null)
                {
                    // the ip address
                    Debug.WriteLine(hostname.CanonicalName);
                }

            }
        }
    }
}

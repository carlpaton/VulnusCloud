using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace VulnusCloud.Domain
{
    // TODO - add this to CarlPaton.Common v1.0.4 ???
    // Quick dirty hack to get the hosts IP

    public class IpAddressService
    {
        public string GetLocalIPv4()
        {
            var output = "";
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType.Equals(NetworkInterfaceType.Ethernet)
                    && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            return ip.Address.ToString();
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Alternative : http://icanhazip.com/
        /// </summary>
        /// <returns></returns>
        public string GetPublicIP()
        {
            return new WebClient()
                .DownloadString("http://checkip.amazonaws.com")
                .Trim();
        }
    }
}

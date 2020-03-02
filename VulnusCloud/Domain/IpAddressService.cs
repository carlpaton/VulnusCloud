using System.Linq;
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
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType.Equals(NetworkInterfaceType.Ethernet)
                    && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    var unicastIPAddressInformation = networkInterface
                        .GetIPProperties()
                        .UnicastAddresses
                        .Where(x => x.Address.AddressFamily.Equals(AddressFamily.InterNetwork))
                        .FirstOrDefault();

                    if (unicastIPAddressInformation != null && unicastIPAddressInformation.Address != null)
                        return unicastIPAddressInformation.Address.ToString();
                }
            }
            return "";
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

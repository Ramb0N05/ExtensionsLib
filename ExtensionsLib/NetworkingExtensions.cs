using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SharpRambo.ExtensionsLib {

    /// <summary>
    /// The NetworkingExtensions class.
    /// </summary>
    public static class NetworkingExtensions {

        #region IPAddress Extensions

        /// <summary>
        /// Determines whether the <see cref="IPAddress" /> is visible in the specified subnet.
        /// </summary>
        /// <param name="address">The ip address.</param>
        /// <param name="cidrNetmask">The netmask as address in <see href="https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing">CIDR-Notation</see>.</param>
        /// <returns>
        ///   <c>True</c> if the <see cref="IPAddress" /> is visible in the specified subnet; otherwise, <c>False</c>.
        /// </returns>
        /// <exception cref="NotSupportedException" />
        /// <exception cref="ArgumentException" />
        public static bool IsInSubnet(this IPAddress address, string cidrNetmask) {
            // See https://stackoverflow.com/a/56461160

            int slashIdx = cidrNetmask.IndexOf('/');
            if (slashIdx == -1) // We only handle netmasks in format "IP/PrefixLength".
                throw new NotSupportedException("Only Subnetmasks with a given prefix length are supported.");

            // First parse the address of the netmask before the prefix length.
            IPAddress maskAddress = IPAddress.Parse(
#if NETCOREAPP3_1_OR_GREATER
                cidrNetmask.AsSpan(0, slashIdx)
#else
                cidrNetmask.Substring(0, slashIdx)
#endif
                );

            if (maskAddress.AddressFamily != address.AddressFamily) // We got something like an IPV4-Address for an IPv6-Mask. This is not valid.
                return false;

            // Now find out how long the prefix is.
            int maskLength = int.Parse(
#if NET5_0_OR_GREATER
                cidrNetmask[(slashIdx + 1)..]
#else
                cidrNetmask.Substring(slashIdx + 1)
#endif
                );

            if (maskLength == 0)
                return true;

            if (maskLength < 0)
                throw new NotSupportedException("A Subnetmask should not be less than 0.");

            if (maskAddress.AddressFamily == AddressFamily.InterNetwork) {
                // Convert the mask address to an unsigned integer.
                uint maskAddressBits = BitConverter.ToUInt32(maskAddress.GetAddressBytes().Reverse().ToArray(), 0);

                // And convert the IpAddress to an unsigned integer.
                uint ipAddressBits = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(), 0);

                // Get the mask/network address as unsigned integer.
                uint mask = uint.MaxValue << (32 - maskLength);

                // https://stackoverflow.com/a/1499284/3085985 Bitwise AND mask and MaskAddress,
                // this should be the same as mask and IpAddress as the end of the mask is 0000
                // which leads to both addresses to end with 0000 and to start with the prefix.
                return (maskAddressBits & mask) == (ipAddressBits & mask);
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetworkV6) {
                // Convert the mask address to a BitArray. Reverse the BitArray to compare the bits
                // of each byte in the right order.
                BitArray maskAddressBits =
#if NET5_0_OR_GREATER
                    new(
#else
                    new BitArray(
#endif
                    maskAddress.GetAddressBytes().Reverse().ToArray());

                // And convert the IpAddress to a BitArray. Reverse the BitArray to compare the bits
                // of each byte in the right order.
                BitArray ipAddressBits =
#if NET5_0_OR_GREATER
                    new(
#else
                    new BitArray(
#endif
                    address.GetAddressBytes().Reverse().ToArray());

                int ipAddressLength = ipAddressBits.Length;

                if (maskAddressBits.Length != ipAddressBits.Length)
                    throw new ArgumentException("Length of IP Address and Subnet Mask do not match.");

                // Compare the prefix bits.
                for (int i = ipAddressLength - 1; i >= ipAddressLength - maskLength; i--) {
                    if (ipAddressBits[i] != maskAddressBits[i])
                        return false;
                }

                return true;
            }

            throw new NotSupportedException("Only InterNetworkV6 or InterNetwork address families are supported.");
        }

        #endregion IPAddress Extensions
    }
}

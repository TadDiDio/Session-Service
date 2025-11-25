using System;
using PurrNet;
using PurrNet.Steam;
using PurrNet.Transports;
using Steamworks;
using Unity.VisualScripting;

namespace SessionService.Sample
{
    public class SteamworksSessionProvider : BaseClientHostProvider
    {
        public SteamworksSessionProvider(NetworkManager network, SteamTransport transport) : base(network, transport) { }

        protected override void AddTransportIfNeeded(NetworkManager network, GenericTransport transport)
        {
            if (!network.TryGetComponent(typeof(SteamTransport), out _))
            {
                network.AddComponent<SteamTransport>();
            }
        }
        
        protected override SessionDetails GetDetails()
        {
            return new SessionDetails
            {
                ServerAddress = SteamUser.GetSteamID().ToString()
            };
        }

        protected override void SetDetails(GenericTransport transport, SessionDetails details)
        {
            if (transport is not SteamTransport steam)
                throw new InvalidCastException("Could not cast transport to steam transport.");

            steam.peerToPeer = true;
            steam.address = details.ServerAddress;
        }
    }
}
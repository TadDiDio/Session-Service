using PurrNet;
using PurrNet.Transports;
using Unity.VisualScripting;

namespace SessionService.Sample
{
    public class LocalSessionProvider : BaseClientHostProvider
    {
        public LocalSessionProvider(NetworkManager network, LocalTransport transport) : base(network, transport) { }
        protected override void AddTransportIfNeeded(NetworkManager network, GenericTransport transport)
        {
            if (!network.TryGetComponent(typeof(LocalTransport), out _))
            {
                network.AddComponent<LocalTransport>();
            }
        }

        protected override SessionDetails GetDetails()
        {
            return new SessionDetails();
        }

        protected override void SetDetails(GenericTransport transport, SessionDetails details)
        {
            // No-op
        }
    }
}
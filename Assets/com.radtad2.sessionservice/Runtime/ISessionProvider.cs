using System;
using System.Threading.Tasks;

namespace SessionService
{
    /// <summary>
    /// Provides services related to connecting to the game server.
    /// </summary>
    public interface ISessionProvider : IAsyncDisposable
    {
        /// <summary>
        /// Initializes this provider.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Connects to the server.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Information about the connection made.</returns>
        public Task<SessionConnection> Connect(SessionRequest request);

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        /// <param name="timeoutSeconds">The number of seconds to try for before failing.</param>
        /// <returns>True if successful.</returns>
        public Task<bool> Disconnect(float timeoutSeconds);
    }
}

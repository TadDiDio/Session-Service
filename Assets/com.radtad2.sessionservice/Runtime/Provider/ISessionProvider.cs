using System;
using System.Threading;
using System.Threading.Tasks;

namespace SessionService
{
    /// <summary>
    /// Provides connecting to a dedicated server session.
    /// </summary>
    public interface ISessionProvider : IDisposable
    {
        /// <summary>
        /// Initializes this provider.
        /// </summary>
        public void Initialize();
        
        /// <summary>
        /// Starts looking for a match.
        /// </summary>
        /// <param name="createRequest">The request details.</param>
        /// <param name="token">A token to cancel the operation.</param>
        public Task<SessionCreateResult> CreateSessionAsync(SessionCreateRequest createRequest, CancellationToken token);

        /// <summary>
        /// Joins an already started session.
        /// </summary>
        /// <param name="request">The session join request.</param>s
        /// <param name="token">A token to cancel the operation.</param>
        /// <returns>The result of joining.</returns>
        public Task<SessionJoinResult> JoinSessionAsync(SessionJoinRequest request, CancellationToken token);
        
        /// <summary>
        /// Disconnects from a dedicated server.
        /// </summary>
        /// <param name="token">A token to cancel the operation.</param>
        public Task DisconnectAsync(CancellationToken token);
    }
}
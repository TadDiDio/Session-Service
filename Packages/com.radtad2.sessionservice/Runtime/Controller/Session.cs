using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SessionService
{
    public static class Session
    {
        public static bool Initialized;
        
        private static SessionController _controller;
        
        private static Action<SessionState> _relay;
        private static CancellationTokenSource _tokenSource;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnEnterPlayMode()
        {
            Application.quitting += Shutdown;
            _tokenSource = new CancellationTokenSource();
        }

        private static void Shutdown()
        {
            Initialized = false;
            Application.quitting -= Shutdown;
            
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            
            if (_controller != null)
            {
                _controller.OnStateChanged -= _relay;
                _controller.Dispose();
            }
        }

        /// <summary>
        /// The current session state.
        /// </summary>
        public static SessionState State => _controller?.State ?? SessionState.Disconnected;
        
        /// <summary>
        /// Invoked when the current session state updates.
        /// </summary>
        public static event Action<SessionState> OnStateChanged;
        
        /// <summary>
        /// Sets a new provider to use. 
        /// </summary>
        /// <param name="provider"></param>
        /// <remarks>This can only be done while in the disconnected state.</remarks>
        public static void SetProvider(ISessionProvider provider)
        {
            if (_controller == null)
            {
                _controller = new SessionController();
                _relay = s => OnStateChanged?.Invoke(s);
                _controller.OnStateChanged += _relay;
                Initialized = true;
            }
            
            _controller.SetProvider(provider);
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="createRequest">The requested session.</param>
        /// <param name="token">A token to cancel the operation.</param>
        /// <returns>The result of creating a session.</returns>
        public static async Task<SessionCreateResult> CreateSessionAsync(SessionCreateRequest createRequest, CancellationToken token)
        {
            if (_controller == null) return SessionCreateResult.Fail();
            
            return await _controller.CreateSessionAsync(createRequest, token);
        }

        /// <summary>
        /// Joins an existing session.
        /// </summary>
        /// <param name="request">The session join request.</param>
        /// <param name="token">A token to cancel the operation.</param>
        /// <returns>The result of joining the session.</returns>
        public static async Task<SessionJoinResult> JoinSessionAsync(SessionJoinRequest request, CancellationToken token)
        {
            if (_controller == null) return SessionJoinResult.Fail();
            
            return await _controller.JoinSessionAsync(request, token);
        }

        /// <summary>
        /// Disconnects from a session.
        /// </summary>
        public static async Task DisconnectAsync()
        {
            if (_controller == null) return;
            
            await _controller.DisconnectAsync(_tokenSource.Token);
        }
    }
}
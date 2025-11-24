using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SessionService
{
    public enum SessionState
    {
        Disconnected,
        Connecting,
        Connected,
        Disconnecting
    }

    public class SessionController : IAsyncDisposable
    {
        public event Action<SessionState> OnStateChanged;
        
        private ISessionProvider _provider;
        
        /// <summary>
        /// The current state of the session controller.
        /// </summary>
        public SessionState State { get; private set; } = SessionState.Disconnected;
        
        /// <summary>
        /// Closes the current provider and initializes the new one.
        /// </summary>
        /// <param name="provider">The new provider.</param>
        /// <returns>True if the operation was successful.</returns>
        public void SetProvider(ISessionProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            
            if (State != SessionState.Disconnected) return;

            _ = DisposeProvider(_provider);

            _provider = provider;
            _provider.Initialize();
        }
        
        /// <summary>
        /// Attempts to start a session.
        /// </summary>
        /// <param name="request">The info related to starting the session.</param>
        /// <returns>Information about the connection made.</returns>
        public async Task<SessionConnection> RequestConnect(SessionRequest request)
        {
            if (_provider == null)
            {
                Debug.LogError("No provider set when RequestConnect was called. Make sure to call Session.SetProvider()");
                return SessionConnection.Failed();
            }
            
            if (State is not SessionState.Disconnected) return SessionConnection.Failed();

            SetState(SessionState.Connecting);
            
            var result = await _provider.Connect(request);

            SetState(result.Connected ? SessionState.Connected : SessionState.Disconnected);

            return result;
        }

        /// <summary>
        /// Attempts to start a session.
        /// </summary>
        /// <param name="timeoutSeconds">The number of seconds to attempt before failing.</param>
        /// <returns>True if successful.</returns>
        public async Task<bool> RequestDisconnect(float timeoutSeconds)
        {
            if (_provider == null)
            {
                Debug.LogError("No provider set when RequestDisconnect was called. Make sure to call Session.SetProvider()");
                return false;
            }

            if (State is SessionState.Disconnected) return true;
            if (State is not SessionState.Connected) return false;
            
            SetState(SessionState.Disconnecting);
            
            var success = await _provider.Disconnect(timeoutSeconds);
            
            SetState(success ? SessionState.Disconnected : SessionState.Connected);

            return success;
        }
		
		public async ValueTask DisposeAsync()
        {
            await DisposeProvider(_provider);
        }

        private void SetState(SessionState state)
        {
            State = state;
            OnStateChanged?.Invoke(State);
        }
        
        private async Task DisposeProvider(ISessionProvider provider)
        {
            if (provider == null) return;

            try
            {
                await provider.DisposeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
   }
}

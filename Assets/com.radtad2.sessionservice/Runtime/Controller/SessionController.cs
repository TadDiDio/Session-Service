using System;
using System.Threading;
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

    public class SessionController : IDisposable
    {
        public event Action<SessionState> OnStateChanged;

        private ISessionProvider _provider;
        
        public SessionState State { get; private set; } = SessionState.Disconnected;
        
        private void SetState(SessionState state)
        {
            State = state;
            OnStateChanged?.Invoke(State);
        }
        public void Dispose()
        {
            using var cts = new CancellationTokenSource(2000);
            _provider?.DisconnectAsync(cts.Token).GetAwaiter().GetResult();
            _provider?.Dispose();
            _provider = null;
            State = SessionState.Disconnected;
            OnStateChanged = null;
        }
        
        public void SetProvider(ISessionProvider provider)
        {
            if (State != SessionState.Disconnected) throw new InvalidOperationException("Cannot set a provider while not disconnected.");
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _provider.Initialize();
        }

        private bool EnsureProvider()
        {
            if (_provider != null) return true;
            
            Debug.LogError("Provider was utilized before being set!");
            return false;
        }
        
        public async Task<SessionCreateResult> CreateSessionAsync(SessionCreateRequest createRequest, CancellationToken token)
        {
            if (!EnsureProvider() || State is not SessionState.Disconnected) return SessionCreateResult.Fail();

            SetState(SessionState.Connecting);
            
            var result = await _provider.CreateSessionAsync(createRequest, token);

            SetState(result.Connected ? SessionState.Connected : SessionState.Disconnected);

            return result;
        }

        public async Task<SessionJoinResult> JoinSessionAsync(SessionJoinRequest request, CancellationToken token)
        {
            if (!EnsureProvider() || State is not SessionState.Disconnected) return SessionJoinResult.Fail();

            SetState(SessionState.Connecting);
            
            var result = await _provider.JoinSessionAsync(request, token);

            SetState(result.Connected ? SessionState.Connected : SessionState.Disconnected);

            return result;
        }

        public async Task DisconnectAsync(CancellationToken token)
        {
            if (!EnsureProvider()) return;

            if (State is SessionState.Disconnected) return;
            if (State is not SessionState.Connected) return;
            
            SetState(SessionState.Disconnecting);
            
            await _provider.DisconnectAsync(token);
            
            SetState(SessionState.Disconnected);
        }
    }
}

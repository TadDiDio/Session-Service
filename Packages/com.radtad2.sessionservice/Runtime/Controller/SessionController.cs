using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
            if (State != SessionState.Disconnected) throw new InvalidOperationException("Cannot set a provider while online.");
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

            try
            {
                SetState(SessionState.Connecting);
                var result = await _provider.CreateSessionAsync(createRequest, token);
                SetState(result.Connected ? SessionState.Connected : SessionState.Disconnected);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                SetState(SessionState.Disconnected);
                return SessionCreateResult.Fail();
            }
        }

        public async Task<SessionJoinResult> JoinSessionAsync(SessionJoinRequest request, CancellationToken token)
        {
            if (!EnsureProvider() || State is not SessionState.Disconnected) return SessionJoinResult.Fail();

            try
            {
                SetState(SessionState.Connecting);
                var result = await _provider.JoinSessionAsync(request, token);
                SetState(result.Connected ? SessionState.Connected : SessionState.Disconnected);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                SetState(SessionState.Disconnected);
                return SessionJoinResult.Fail();
            }
        }

        public async Task DisconnectAsync(CancellationToken token)
        {
            if (!EnsureProvider()) return;

            if (State is not SessionState.Connected) return;

            try
            {
                SetState(SessionState.Disconnecting);
                await _provider.DisconnectAsync(token);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                SetState(SessionState.Disconnected);
            }
        }
    }
}

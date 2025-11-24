// using System;
// using System.Data;
// using System.Threading.Tasks;
//
// namespace SessionService.Sample
// {
//     public class ClientHostSessionProvider : ISessionProvider
//     {
//         public ClientHostSessionProvider()
//         {
//             
//         }
//         
//         public override async Task<bool> Disconnect(float timeoutSeconds)
//         {
//             if (!Network.isClient && !Network.isServer) return true;
//
//             void ListenForDisconnect(ConnectionState state, TaskCompletionSource<bool> tcs)
//             {
//                 if (state is ConnectionState.Disconnected)
//                 {
//                     tcs.TrySetResult(true);
//                 }
//             }
//
//             if (Network.isClient)
//             {
//                 var clientTcs = new TaskCompletionSource<bool>();
//                 Action<ConnectionState> clientCallback = state => ListenForDisconnect(state, clientTcs);
//                 try
//                 {
//                     Network.onClientConnectionState += clientCallback;
//                     Network.StopClient();
//
//                     var delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
//                     var completed = await Task.WhenAny(delayTask, clientTcs.Task);
//
//                     if (completed == delayTask || !await clientTcs.Task) return false;
//                 }
//                 catch (Exception e)
//                 {
//                     Log.Exception(e);
//                     return false;
//                 }
//                 finally
//                 {
//                     Network.onClientConnectionState -= clientCallback;
//                 }
//             }
//
//             if (Network.isServer)
//             {
//                 var serverTcs = new TaskCompletionSource<bool>();
//                 Action<ConnectionState> serverCallback = state => ListenForDisconnect(state, serverTcs);
//                 try
//                 {
//                     Network.onServerConnectionState += serverCallback;
//                     Network.StopServer();
//
//                     var delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
//                     var completed = await Task.WhenAny(delayTask, serverTcs.Task);
//
//                     if (completed == delayTask || !await serverTcs.Task) return false;
//                 }
//                 catch (Exception e)
//                 {
//                     Log.Exception(e);
//                     return false;
//                 }
//                 finally
//                 {
//                     Network.onServerConnectionState -= serverCallback;
//                 }
//             }
//
//             return true;
//         }
//
//         public override async Task<SessionConnection> Connect(StartSessionInfo startSessionInfo)
//         {
//             var timeout = startSessionInfo.TimeoutSeconds;
//
//             if (startSessionInfo.LobbyOwner)
//             {
//                 var serverTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
//                 void ServerCallback(ConnectionState state) => ListenForConnection(state, serverTcs);
//                 if (!await TryConnect(true, timeout, ServerCallback, serverTcs))
//                 {
//                     Network.StopServer(); // Clean up a lingering attempt
//                     return SessionConnection.Failed();
//                 }
//             }
//
//             var clientTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
//             void ClientCallback(ConnectionState state) => ListenForConnection(state, clientTcs);
//             if (!await TryConnect(false, timeout, ClientCallback, clientTcs))
//             {
//                 Network.StopClient(); // Clean up a lingering attempt
//                 return SessionConnection.Failed();
//             }
//
//             var type = startSessionInfo.LobbyOwner ? SessionConnectionType.Host : SessionConnectionType.Client;
//             return SessionConnection.Success(type);
//         }
//
//         private void ListenForConnection(ConnectionState state, TaskCompletionSource<bool> tcs)
//         {
//             switch (state)
//             {
//                 case ConnectionState.Connected:
//                     tcs.TrySetResult(true);
//                     break;
//                 case ConnectionState.Disconnected:
//                     tcs.TrySetResult(false);
//                     break;
//             }
//         }
//
//         private async Task<bool> TryConnect(bool server, float timeout, Action<ConnectionState> callback, TaskCompletionSource<bool> tcs)
//         {
//             try
//             {
//                 if (server)
//                 {
//                     Network.onServerConnectionState += callback;
//                     Network.StartServer();
//                 }
//                 else
//                 {
//                     Network.onClientConnectionState += callback;
//                     Network.StartClient();
//                 }
//
//                 var delayTask = Task.Delay(TimeSpan.FromSeconds(timeout));
//                 var completedTask = await Task.WhenAny(delayTask, tcs.Task);
//
//                 return completedTask == tcs.Task && await tcs.Task;
//             }
//             catch (Exception e)
//             {
//                 Log.Exception(e);
//                 return false;
//             }
//             finally
//             {
//                 if (server) Network.onServerConnectionState -= callback;
//                 else Network.onClientConnectionState -= callback;
//             }
//         }
//
//         public override async ValueTask DisposeAsync()
//         {
//             await Disconnect(5);
//         }
//     }
// }
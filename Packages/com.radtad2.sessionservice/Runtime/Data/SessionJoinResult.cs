namespace SessionService
{
    /// <summary>
    /// Result of joining a session.
    /// </summary>
    public class SessionJoinResult
    {
        /// <summary>
        /// Whether the client was connected.
        /// </summary>
        public readonly bool Connected;
        
        private SessionJoinResult(bool connected)
        {
            Connected = connected;
        }
        
        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <remarks>Not all providers will use both the serverAddress and port.</remarks>
        public static SessionJoinResult Success() => new(true);

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        public static SessionJoinResult Fail() => new(false);
    }
}
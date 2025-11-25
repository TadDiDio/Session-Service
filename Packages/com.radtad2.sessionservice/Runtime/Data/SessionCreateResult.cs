namespace SessionService
{
    /// <summary>
    /// Result of starting a session.
    /// </summary>
    public class SessionCreateResult
    {
        /// <summary>
        /// Whether the client was connected.
        /// </summary>
        public bool Connected;

        /// <summary>
        /// Details about how to connect to this session
        /// </summary>
        public SessionDetails SessionDetails;

        private SessionCreateResult(bool connected, SessionDetails details)
        {
            Connected = connected;
            SessionDetails = details;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <param name="details">The session details.</param>
        /// <remarks>Not all providers will use both the serverAddress and port.</remarks>
        public static SessionCreateResult Success(SessionDetails details) => new(true, details);

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        public static SessionCreateResult Fail() => new(false, null);
    }
}
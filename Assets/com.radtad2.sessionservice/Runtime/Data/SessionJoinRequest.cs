namespace SessionService
{
    /// <summary>
    /// A request to join a session.
    /// </summary>
    public class SessionJoinRequest
    {
        /// <summary>
        /// How many seconds to wait before timing out.
        /// </summary>
        public float TimeoutSeconds;

        /// <summary>
        /// Details on the session to connect to.
        /// </summary>
        public SessionDetails SessionDetails;
    }
}
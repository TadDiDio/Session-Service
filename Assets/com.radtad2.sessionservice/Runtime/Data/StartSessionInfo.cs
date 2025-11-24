namespace SessionService
{
    /// <summary>
    /// Information related to starting a session.
    /// </summary>
    public struct SessionRequest
    {
        /// <summary>
        /// If this request is as the session leader. Normally this is a lobby owner. 
        /// </summary>
        public bool SessionLeader;

        /// <summary>
        /// The maximum seconds to wait before giving up.
        /// </summary>
        public float TimeoutSeconds;
    }
}

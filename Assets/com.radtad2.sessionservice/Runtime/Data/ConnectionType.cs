namespace SessionService
{
    /// <summary>
    /// Types of session connection.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// No connection is held.
        /// </summary>
        None,
        
        /// <summary>
        /// Connected as only a client.
        /// </summary>
        Client,
        
        /// <summary>
        /// Connected as only a server.
        /// </summary>
        Server,
        
        /// <summary>
        /// Connected as both client and server.
        /// </summary>
        Host
    }
}
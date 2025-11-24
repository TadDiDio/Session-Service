namespace SessionService
{
	/// <summary>
	/// A connection result.
	/// </summary>
    public class SessionConnection
    {
		/// <summary>
		/// Tells if you are connected.
		/// </summary>
        public readonly bool Connected;

		/// <summary>
		/// Tells what type of connection was made.
		/// </summary>
        public readonly ConnectionType Type;
		
		/// <summary>
		/// The payload with data about the connection.
		/// </summary>
		public ConnectionPayload Payload;
		
        private SessionConnection(bool connected, ConnectionType type, ConnectionPayload payload = null)
        {
            Connected = connected;
            Type = type;
            Payload = payload;
        }

		/// <summary>
		/// Creates a failed connection result.
		/// </summary>
        public static SessionConnection Failed()
        {
            return new SessionConnection(false, ConnectionType.None);
        }

        /// <summary>
        /// Creates a successful connection result.
        /// </summary>
        /// <param name="connectionType">The type of connection made.</param>
        /// <param name="payload">The payload associated with this success.</param>
        public static SessionConnection Success(ConnectionType connectionType, ConnectionPayload payload)
        {
            return new SessionConnection(true, connectionType, payload);
        }
    }
}

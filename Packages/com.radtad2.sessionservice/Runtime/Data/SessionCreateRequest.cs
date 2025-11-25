using System.Collections.Generic;

namespace SessionService
{
    public class SessionCreateRequest
    {
        /// <summary>
        /// The seconds to wait before timing out.
        /// </summary>
        public float TimeoutSeconds;

        /// <summary>
        /// Game specific match options.
        /// </summary>
        public IReadOnlyDictionary<string, string> Options => _options;

        private Dictionary<string, string> _options = new();
        
        /// <summary>
        /// Sets an option.
        /// </summary>
        /// <param name="key">The option to set.</param>
        /// <param name="value">The value.</param>
        public void SetOption(string key, string value) => _options[key] = value;
        
        /// <summary>
        /// Sets an option.
        /// </summary>
        /// <param name="key">The option to set.</param>
        /// <param name="value">The value.</param>
        public void SetOption(string key, int value) => _options[key] = value.ToString();
        
        /// <summary>
        /// Sets an option.
        /// </summary>
        /// <param name="key">The option to set.</param>
        /// <param name="value">The value.</param>
        public void SetOption(string key, bool value) => _options[key] = value.ToString();
    }
}
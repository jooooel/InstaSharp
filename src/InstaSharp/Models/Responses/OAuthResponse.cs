using Newtonsoft.Json;

namespace InstaSharp.Models.Responses
{
    /// <summary>
    /// OAuthResponse
    /// </summary>
    public class OAuthResponse
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public UserInfo User { get; set; }
        /// <summary>
        /// Gets or sets the access_ token.
        /// </summary>
        /// <value>
        /// The access_ token.
        /// </value>
        [JsonProperty("Access_Token")]
        public string AccessToken { get; set; }

        [JsonProperty("code")]
        public int? ErrorCode { get; set; }

        [JsonProperty("error_type")]
        public string ErrorType { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}

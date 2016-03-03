using System.Collections.Generic;

namespace InstaSharp.Models.Responses
{
    /// <summary>
    /// List All Subscriptions Response
    /// </summary>
    public class ListAllSubscriptionsResponse : Response
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable<Subscription> Data { get; set; }
    }
}

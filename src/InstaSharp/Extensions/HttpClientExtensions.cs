using System.Linq;
using System.Net;
using InstaSharp.Models.Responses;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using InstaSharp.Models;

namespace InstaSharp.Extensions
{
    internal static class HttpClientExtensions
    {
        private const string RateLimitRemainingHeader = "X-Ratelimit-Remaining";
        private const string RateLimitHeader = "X-Ratelimit-Limit";

        public static async Task<T> ExecuteAsync<T>(this HttpClient client, HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            string resultData = await response.Content.ReadAsStringAsync();

            try
            {
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    // This is something that happens from now and then when calling the Instagram API
                    // It probably has to do with some kind of overload on Instagrams side
                    // Throw an exception with InstaSharpExceptionType.InstagramApiUnavailable and let the caller handle it
                    throw new InstaSharpException(string.Format("Instagram API is unavailable. Failed to execute request: {0}. Response: {1}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response)), InstaSharpExceptionType.InstagramApiUnavailable);
                }

                var result = JsonConvert.DeserializeObject<T>(resultData);

                var endpointResponse = result as Response;

                if (endpointResponse != null)
                {
                    if (response.Headers.Contains(RateLimitHeader))
                    {
                        endpointResponse.RateLimitLimit =
                            response.Headers
                                .GetValues(RateLimitHeader)
                                .Select(int.Parse)
                                .SingleOrDefault();
                    }

                    if (response.Headers.Contains(RateLimitRemainingHeader))
                    {
                        endpointResponse.RateLimitRemaining =
                            response.Headers
                                .GetValues(RateLimitRemainingHeader)
                                .Select(int.Parse)
                                .SingleOrDefault();
                    }
                }

                return result;
            }
            catch (JsonReaderException exception)
            {
                throw new InstaSharpException(string.Format("Response: {0}. Failed to parse {1}", JsonConvert.SerializeObject(response), resultData), exception);
            }
        }
    }
}

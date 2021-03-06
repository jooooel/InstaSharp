﻿using InstaSharp.Extensions;
using InstaSharp.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstaSharp
{
    /// <summary>
    /// OAuth authentication
    /// </summary>
    public class OAuth
    {
        private readonly InstagramConfig config;

        /// <summary>
        /// Response Type
        /// </summary>
        public enum ResponseType
        {
            /// <summary>
            /// The code
            /// </summary>
            Code,
            /// <summary>
            /// The token
            /// </summary>
            Token
        }

        /// <summary>
        /// Scope
        /// </summary>
        public enum Scope
        {
            /// <summary>
            /// basic
            /// </summary>
            Basic,
            /// <summary>
            /// public_content
            /// </summary>
            Public_Content,
            /// <summary>
            /// follower_list
            /// </summary>
            Follower_List,
            /// <summary>
            /// comments
            /// </summary>
            Comments,
            /// <summary>
            /// relationships
            /// </summary>
            Relationships,
            /// <summary>
            /// likes
            /// </summary>
            Likes
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public OAuth(InstagramConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Authentications the link.
        /// </summary>
        /// <param name="instagramOAuthUri">The instagram o authentication URI.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="callbackUri">The callback URI.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="state">Optional parameter to "carry through a server-specific state"</param>
        /// <returns>The authentication url</returns>
        public static string AuthLink(string instagramOAuthUri, string clientId, string callbackUri, List<Scope> scopes, ResponseType responseType = ResponseType.Token, string state = null)
        {
            var scopesForUri = BuildScopeForUri(scopes);
            return BuildAuthUri(instagramOAuthUri, clientId, callbackUri, responseType, scopesForUri, state);
        }

        /// <summary>
        /// Get authentication link.
        /// </summary>
        /// <param name="config">Instagram configuration.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="state">Optional parameter to "carry through a server-specific state"</param>
        /// <returns>The authentication url</returns>
        public static string AuthLink(InstagramConfig config, List<Scope> scopes, ResponseType responseType = ResponseType.Token, string state = null)
        {
            var scopesForUri = BuildScopeForUri(scopes);
            return BuildAuthUri(config.OAuthUri.TrimEnd('/') + "/authorize", 
                config.ClientId, 
                config.RedirectUri, 
                responseType, 
                scopesForUri,
                state);
        }

        /// <summary>
        /// Requests the token.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>An OAuthResponse object</returns>
        public Task<OAuthResponse> RequestToken(string code)
        {
            var client = new HttpClient { BaseAddress = new Uri(config.OAuthUri) };
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(client.BaseAddress, "access_token"));

            var multipartFormDataContent = new MultipartFormDataContent
            {
                {new StringContent(config.ClientId), "client_id"},
                {new StringContent(config.ClientSecret), "client_secret"},
                {new StringContent("authorization_code"), "grant_type"},
                {new StringContent(config.RedirectUri), "redirect_uri"},
                {new StringContent(code), "code"},
            };
            request.Content = multipartFormDataContent;

            return client.ExecuteAsync<OAuthResponse>(request);

        }

        /// <summary>
        /// Build scope string for auth uri.
        /// </summary>
        /// <param name="scopes">List of scopes.</param>
        /// <returns>Comma separated scopes.</returns>
        private static string BuildScopeForUri(List<Scope> scopes)
        {
            var scope = new StringBuilder();

            foreach (var s in scopes)
            {
                if (scope.Length > 0)
                {
                    scope.Append("+");
                }
                scope.Append(s);
            }

            return scope.ToString();
        }

        /// <summary>
        /// Build authentication link.
        /// </summary>
        /// <param name="instagramOAuthUri">The instagram o authentication URI.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="callbackUri">The callback URI.</param>
        /// <param name="scopes">The scopes.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="state">Optional parameter to "carry through a server-specific state"</param>
        /// <returns>The authentication uri</returns>
        private static string BuildAuthUri(string instagramOAuthUri, string clientId, string callbackUri, ResponseType responseType, string scopes, string state = null)
        {
            var authUri = string.Format("{0}?client_id={1}&redirect_uri={2}&response_type={3}&scope={4}", new object[] {
                instagramOAuthUri.ToLower(),
                clientId.ToLower(), 
                callbackUri, 
                responseType.ToString().ToLower(),
                scopes.ToLower()
            });

            if (state != null)
            {
                authUri = $"{authUri}&state={state}";
            }

            return authUri;
        }
    }
}

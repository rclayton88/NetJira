using System;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Http;
using DotNetOpenAuth.OAuth;
using System.Threading.Tasks;
using System.Text;

namespace NetJira
{
    /// <summary>
    /// Handles making REST requests to Jira.
    /// </summary>
    public abstract class JiraRest
    {
        private OAuth1RsaSha1HttpMessageHandler jiraConsumer = new OAuth1RsaSha1HttpMessageHandler();
        private readonly string baseUrl;
        public JiraRest(string baseUrl,
            string accessToken,
            string accessTokenSecret,
            string consumerKey,
            string pathToSigningCertificate,
            string signingCertificatePassword)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this.baseUrl = baseUrl;
            jiraConsumer.AccessToken = accessToken;
            jiraConsumer.AccessTokenSecret = accessTokenSecret;
            jiraConsumer.ConsumerKey = consumerKey;
            jiraConsumer.SigningCertificate = new X509Certificate2(pathToSigningCertificate, signingCertificatePassword);
            jiraConsumer.Location = OAuth1HttpMessageHandlerBase.OAuthParametersLocation.AuthorizationHttpHeader;
        }

        /// <summary>
        /// Sends GET request to JIRA REST API end point.
        /// </summary>
        /// <param name="restEndPoint">Rest API Endpoint</param>
        /// <returns>Status code and content body. 500 response if an exception occurred client-sie and the error message.</returns>
        public Tuple<HttpStatusCode, string> Get(string restEndPoint)
        {
            return makeRequest(HttpMethod.Get, restEndPoint);
        }

        public Tuple<HttpStatusCode, string> Post(string restEndPoint, string postMessage)
        {
            return makeRequest(HttpMethod.Post, restEndPoint, postMessage);
        }

        public Tuple<HttpStatusCode, string> MakeIssue(string restEndPoint)
        {
            throw new NotImplementedException();
        }

        private Tuple<HttpStatusCode, string> makeRequest(HttpMethod httpMethod, 
            string restEndPoint, 
            string contentBody = null)
        {
            var jiraRequest = new HttpRequestMessage(httpMethod, this.baseUrl + restEndPoint);
            jiraConsumer.ApplyAuthorization(jiraRequest);
            if (contentBody != null) 
            { 
                jiraRequest.Content = new StringContent(contentBody, Encoding.UTF8, "application/json");
            }

            HttpClient client = new HttpClient();
            try
            {
                var responseMessage = client.SendAsync(jiraRequest).AwaitResult();
                var content = responseMessage.Content.ReadAsStringAsync().AwaitResult<string>();
                return new Tuple<HttpStatusCode, string>(responseMessage.StatusCode, content);
            }
            catch (Exception ex)
            {
                return new Tuple<HttpStatusCode, string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
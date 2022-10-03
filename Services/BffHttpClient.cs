using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Web;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class BffHttpClient : HttpClient
    {
        protected readonly ILogger _logger;
        protected readonly IConfiguration _configuration;
        protected Dictionary<string, string> _headers = new();
        protected readonly string _baseUrl;

        public string Headers
        { get; set; }

        private static HttpClientHandler GetHttpClientHandler()
        {
            HttpClientHandler handler = new();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (
                httpRequestMessage, 
                cert, 
                cetChain, 
                policyErrors) =>
                {
                    return true;
                };
            return handler;
        }

        public BffHttpClient(
            string baseUrl, 
            object headers, 
            ILogger logger) 
            : base(GetHttpClientHandler())
        {
            _baseUrl = baseUrl;
            foreach (PropertyInfo headerInfo in headers.GetType().GetProperties())
            {
                _headers.Add(headerInfo.Name, headerInfo.GetValue(headerInfo).ToString());
            }
            _logger = logger;
        }

        public HttpResponseMessage Send(
            string path, 
            HttpMethod method, 
            dynamic content)
        {
            HttpRequestMessage toSend = new();
            toSend.RequestUri = new($"{_baseUrl}{path}");
            toSend.Method = method;
            foreach (KeyValuePair<string, string> headers in _headers)
            {
                toSend.Headers.Add(headers.Key, headers.Value);
            }
            toSend.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            
            _logger.LogInformation($"Sending request to URL: {toSend.RequestUri}");
            HttpResponseMessage response = base.Send(toSend);
            if (!response.IsSuccessStatusCode)
            {
                string responseErrorContent = response.Content.ReadAsStringAsync().Result;
                string responseErrorMessage = LogError(path, response, responseErrorContent);
                throw new BffHttpException(response.StatusCode, responseErrorMessage);
            }
            return response;
        }

        public BackendResponse Delete(
            string path, 
            dynamic content)
        {
            HttpResponseMessage deleteResponse = Send(path, HttpMethod.Delete, content);
            return new BackendResponse(deleteResponse.StatusCode, deleteResponse.Content.ReadAsStringAsync().Result, deleteResponse);
        }

        public BackendResponse Get(string path)
        {
            HttpResponseMessage getResponse = Send(path, HttpMethod.Get, null);
            return new BackendResponse(getResponse.StatusCode, getResponse.Content.ReadAsStringAsync().Result, getResponse);
        }

        public BackendResponse Get(
            string path, 
            object queryParams)
        {
            List<string> stringParams = new();
            foreach (PropertyInfo param in queryParams.GetType().GetProperties())
            {
                stringParams.Add($"{param.Name}={HttpUtility.UrlEncode(param.GetValue(queryParams).ToString())}");
            }
            path = string.Concat(path, "?", string.Join('&', stringParams));
            return Get(path);
        }

        public BackendResponse Post(
            string path, 
            dynamic content)
        {
            HttpResponseMessage postResponse = Send(path, HttpMethod.Post, content);
            return new BackendResponse(postResponse.StatusCode, postResponse.Content.ReadAsStringAsync().Result, postResponse);
        }

        public BackendResponse Put(
            string path, 
            dynamic content)
        {
            HttpResponseMessage postResponse = Send(path, HttpMethod.Put, content);
            return new(postResponse.StatusCode, postResponse.Content.ReadAsStringAsync().Result, postResponse);
        }

        public string LogError(
            string path, 
            HttpResponseMessage httpResponse, 
            string message)
        {
            string errorMessage = $"Error has occurred in \"{httpResponse.RequestMessage.Method}\" request to \"{_baseUrl}{path}\" "
                                + $"with status code \"{(int)httpResponse.StatusCode}\" and message \"{message}\"";
            _logger.LogError(errorMessage);
            return errorMessage;
        }
    }
}

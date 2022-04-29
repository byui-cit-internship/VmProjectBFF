using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public class Backend
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private string _cookie;

        public string Cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }

        public Backend(IHttpContextAccessor httpContextAccessor, ILogger logger, IConfiguration configuration)
            : this(logger, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Backend(string cookie, ILogger logger, IConfiguration configuration)
            : this(logger, configuration)
        {
            _cookie = cookie;
        }

        public Backend(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _cookie = "";
        }

        public HttpResponseMessage Send(string path, HttpMethod method, dynamic content)
        {
            HttpRequestMessage toSend = new();
            toSend.RequestUri = new Uri($"{_configuration.GetConnectionString("BackendRootUri")}{path}");
            toSend.Method = method;
            toSend.Headers.Add(HeaderNames.Cookie,
                _cookie != null && _cookie != "" ? _cookie : _httpContextAccessor.HttpContext.Session.GetString("BESessionCookie"));
            toSend.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.Send(toSend);
            if (!response.IsSuccessStatusCode)
            {
                string responseErrorContent = response.Content.ReadAsStringAsync().Result;
                string responseErrorMessage = LogError(path, response, responseErrorContent);
                throw new BackendException(response.StatusCode, responseErrorMessage);
            }
            if (response.Headers.Contains("Set-Cookie"))
            {
                string cookieHeader = response.Headers.GetValues("Set-Cookie")?.ToArray()[0];
                _httpContextAccessor.HttpContext.Session.SetString("BESessionCookie", cookieHeader.Split(';', 2)[0]);
            }
            return response;
        }

        public BackendResponse Delete(string path, dynamic content)
        {
            HttpResponseMessage deleteResponse = Send(path, HttpMethod.Delete, content);
            return new BackendResponse(deleteResponse.StatusCode, deleteResponse.Content.ReadAsStringAsync().Result, deleteResponse);
        }

        public BackendResponse Get(string path)
        {
            HttpResponseMessage getResponse = Send(path, HttpMethod.Get, null);
            return new BackendResponse(getResponse.StatusCode, getResponse.Content.ReadAsStringAsync().Result, getResponse);
        }

        public BackendResponse Get(string path, object queryParams)
        {
            List<string> stringParams = new();
            foreach (PropertyInfo param in queryParams.GetType().GetProperties())
            {
                stringParams.Add($"{param.Name}={HttpUtility.UrlEncode(param.GetValue(queryParams).ToString())}");
            }
            path = string.Concat(path,"?", string.Join('&', stringParams));
            return Get(path);
        }

        public BackendResponse Post(string path, dynamic content)
        {
            HttpResponseMessage postResponse = Send(path, HttpMethod.Post, content);
            return new BackendResponse(postResponse.StatusCode, postResponse.Content.ReadAsStringAsync().Result, postResponse);
        }

        public BackendResponse Put(string path, dynamic content)
        {
            HttpResponseMessage postResponse = Send(path, HttpMethod.Put, content);
            return new BackendResponse(postResponse.StatusCode, postResponse.Content.ReadAsStringAsync().Result, postResponse);
        }

        public string LogError(string path, HttpResponseMessage httpResponse, string message)
        {
            string errorMessage = $"Error has occurred in \"{httpResponse.RequestMessage.Method}\" request to \"{_configuration.GetConnectionString("BackendRootUri")}{path}\" "
                                + $"with status code \"{(int)httpResponse.StatusCode}\" and message \"{message}\"";
            _logger.LogError(errorMessage);
            return errorMessage;
        }
    }
}

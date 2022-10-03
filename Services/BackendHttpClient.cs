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
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class BackendHttpClient : BffHttpClient
    {
        private string _cookie;

        public string Cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }

        protected static string GetBaseUrl(IConfiguration configuration)
        {
            return configuration.GetConnectionString("BackendRootUri");
        }

        public BackendHttpClient(
            IConfiguration configuration,
            ILogger logger,
            string vimaCookieValue)
            : base(GetBaseUrl(configuration),
                  new { cookie = $"vima-cookie={vimaCookieValue}" },
                  logger)
        {
        }

        new public BackendResponse Delete(
            string path,
            dynamic content)
        {
            try
            {
                return base.Delete(
                    path,
                    (object)content);
            } catch (BffHttpException ex)
            {
                throw new BackendHttpException(ex.StatusCode, ex.Message);
            }
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

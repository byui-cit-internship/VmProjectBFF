using System;
using System.Net;

public class BackendException : Exception
{
    private HttpStatusCode _statusCode;
    private string _message;

    public HttpStatusCode StatusCode { get { return _statusCode; } private set { _statusCode = value; } }
    public string Message { get { return _message; } private set { _message = value; } }

    public BackendException(HttpStatusCode statusCode, string message)
    {
        _statusCode = statusCode;
        _message = message;
    }
}
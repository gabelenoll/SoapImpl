using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

public abstract class ServiceBase
{
    private HttpClient _httpClient;
    private string _url;

    protected ServiceBase(HttpClient httpClient, string? url)
    {
        _httpClient = httpClient;
        if(url == null)
            throw new NullReferenceException("Service URL is not defined.");
        _url = url;
    }

    protected virtual void PrepareContent<T>(StreamContent content, RequestMessage<T> requestMessage, string url){}

    protected async Task<TResponseMessage> SendMessageAsync<TResponseMessage, TParameters>(RequestMessage<TParameters> requestMessage, TParameters parameters)
        where TResponseMessage : IServiceResponseMessage, new()
    {
        using(MemoryStream requestStream = new MemoryStream())
        {
            using(XmlWriter writer = XmlWriter.Create(requestStream))
                requestMessage.WriteTo(writer, _url, parameters);
            requestStream.Seek(0, SeekOrigin.Begin);
            
            using(StreamContent content = new StreamContent(requestStream))
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestMessage.GetMethodUrl(_url));
                content.Headers.Add("Content-Type", "text/xml");
                PrepareContent(content, requestMessage, _url);
                httpRequestMessage.Content = content;
                HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);
                
                TResponseMessage ret = new TResponseMessage();
                CheckStatusCode(httpResponseMessage.StatusCode, ret);
                if(ret.Error == null)
                    using(Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync())
                        ret.Read(responseStream, requestMessage);
                return ret;
            }
        }
    }

    protected virtual void CheckStatusCode(HttpStatusCode statusCode, IServiceResponseMessage responseMessage)
    {
        if((int)statusCode != StatusCodes.Status200OK)
            responseMessage.SetError(String.Format("The status code from the server was {0}. Not parsing the response.", (int)statusCode));
    }

    protected ServiceOutcome<TResult> GetOutcome<TResult>(ResponseMessage<TResult> responseMessage) where TResult : notnull
    {
        string? error = responseMessage.Error;
        return error == null
            ? new ServiceOutcome<TResult>(responseMessage.Result)
            : new ServiceOutcome<TResult>(error);
    }

    protected HttpClient HttpClient{get{return _httpClient;}}
    protected string Url{get{return Url;}}
}
using System;
using System.Net.Http;
using Voith.WF.OrderReleaseVP.Business.Defintions;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed partial class WFHelperSharePointSoapService : SOAP.ServiceBase
{
    private static string GetUrl(AppSettings settings)
    {
        string? ret = settings?.Business?.Repositories?.WFHelper?.Soap?.SharePointUrl;
        if(ret == null)
            throw new NullReferenceException("WFHelper service SharePoint URL is not defined.");
        return ret;
    }

    private static string GetSoapActionBase(AppSettings settings)
    {
        string? ret = settings?.Business?.Repositories?.WFHelper?.Soap?.SharePointSoapActionBase;
        if(ret == null)
            throw new NullReferenceException("WFHelper service SharePoint SoapActionBase is not defined.");
        return ret;
    }

    private string _soapActionBase;

    public WFHelperSharePointSoapService(HttpClient httpClient, AppSettings settings) :
        base(httpClient, GetUrl(settings))
    {
        _soapActionBase = GetSoapActionBase(settings);
    }

    protected override void PrepareContent<T>(StreamContent content, SOAP.RequestMessage<T> requestMessage, string serviceUrl)
    {
        content.Headers.Add("SOAPAction", $"{_soapActionBase}{requestMessage.Name}");
    }
}
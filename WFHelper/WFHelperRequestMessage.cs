using System.Xml.Linq;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal abstract class WFHelperRequestMessage<TParameters> : SOAP.RequestMessage<TParameters>
{
    protected WFHelperRequestMessage(string name) :
        base(name)
    {
    }

    protected abstract void FillBody(XElement body, TParameters parameters);

    private XElement MakeBody(TParameters parameters)
    {
        XElement ret = new XElement(XnEnvelope + "Body");
        FillBody(ret, parameters);
        return ret;
    }

    protected override XElement MakeEnvelope(string url, TParameters parameters)
    {
        XElement ret = new XElement(XnEnvelope + "Envelope");
        ret.Add(new XAttribute(XNamespace.Xmlns + "s", NS_ENVELOPE));
        ret.Add(MakeBody(parameters));
        return ret;
    }

    public override string GetMethodUrl(string serviceUrl)
    {
        return serviceUrl;
    }
}
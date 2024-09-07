using System.Xml;
using System.Xml.Linq;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

public abstract class RequestMessage<TParameters> : MessageBase, IServiceRequestMessage
{
    protected static XElement MakeTextElement(string name, string text)
    {
        XElement ret = new XElement(name);
        ret.Add(new XText(text));
        return ret;
    }

    private string _name;
    
    protected RequestMessage(string name)
    {
        _name = name;
    }

    protected abstract XElement MakeEnvelope(string url, TParameters parameters);

    public void WriteTo(XmlWriter writer, string url, TParameters parameters)
    {
        XDocument doc = new XDocument();
        doc.Add(MakeEnvelope(url, parameters));
        doc.WriteTo(writer);
    }

    public abstract string GetMethodUrl(string serviceUrl);

    public string Name{get{return _name;}}
}
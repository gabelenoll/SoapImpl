using System.Xml.Linq;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

public abstract class MessageBase
{
    protected const string NS_ENVELOPE = "http://schemas.xmlsoap.org/soap/envelope/";
    protected const string NS_PAYLOAD_ORVP = "http://www.voith.com/Workflow/WebService/OrderReleaseVP";
    protected const string NS_PAYLOAD_SHAREPOINT = "http://schemas.microsoft.com/sharepoint/soap/";
    protected const string NS_SHAREPOINT_ROWSET = "urn:schemas-microsoft-com:rowset";
    protected const string NS_SHAREPOINT_ROWSET_SCHEMA = "#RowsetSchema";

    private static XNamespace _xnEnvelope;
    private static XNamespace _xnPayloadOrvp;
    private static XNamespace _xnPayloadSharePoint;
    private static XNamespace _xnSharePointRowset;
    private static XNamespace _xnSharePointRowsetSchema;

    static MessageBase()
    {
        _xnEnvelope = NS_ENVELOPE;
        _xnPayloadOrvp= NS_PAYLOAD_ORVP;
        _xnPayloadSharePoint = NS_PAYLOAD_SHAREPOINT;
        _xnSharePointRowset = NS_SHAREPOINT_ROWSET;
        _xnSharePointRowsetSchema = NS_SHAREPOINT_ROWSET_SCHEMA;
    }

    protected static XNamespace XnEnvelope{get{return _xnEnvelope;}}
    protected static XNamespace XnPayloadOrvp{get{return _xnPayloadOrvp;}}
    protected static XNamespace XnPayloadSharePoint{get{return _xnPayloadSharePoint;}}
    protected static XNamespace XnSharePointRowset{get{return _xnSharePointRowset;}}
    protected static XNamespace XnSharePointRowsetSchema{get{return _xnSharePointRowsetSchema;}}
}
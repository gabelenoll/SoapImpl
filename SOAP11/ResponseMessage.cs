using System.IO;
using System.Xml;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

public abstract class ResponseMessage<TResult> : MessageBase, IServiceResponseMessage where TResult: notnull
{
    static ResponseMessage()
    {
        XmlNamespaceManager namespaceManager = XMethods.NamespaceManager;
        namespaceManager.AddNamespace("s", NS_ENVELOPE);
        namespaceManager.AddNamespace("porvp", NS_PAYLOAD_ORVP);
        namespaceManager.AddNamespace("psp", NS_PAYLOAD_SHAREPOINT);
        namespaceManager.AddNamespace("sprs", NS_SHAREPOINT_ROWSET);
        namespaceManager.AddNamespace("sprss", NS_SHAREPOINT_ROWSET_SCHEMA);
    }

    private string? _error;
    private TResult? _result;

    protected ResponseMessage()
    {
        _error = null;
        _result = default;
    }

    protected void SetResult(TResult? result){_result = result;}

    public abstract void Read(Stream stream, IServiceRequestMessage requestMessage);
    public void SetError(string? error){_error = error;}
    public string? Error{get{return _error;}}

    public TResult Result
    {
        get
        {
            if(_result == null)
                throw new InvalidDataException("The result of the response message could not be determined.");
            return _result;
        }
    }
}
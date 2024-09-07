using System.IO;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

public interface IServiceResponseMessage
{
    void Read(Stream stream, IServiceRequestMessage requestMessage);

    void SetError(string? error);
    string? Error{get;}
}
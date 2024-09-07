namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

public sealed class ServiceOutcome<TResult>
{
    private string? _error;
    private TResult? _result;

    internal ServiceOutcome(string error){_error = error;}
    internal ServiceOutcome(TResult result){_result = result;}

    public string? Error {get{return _error;}}
    public TResult? Result {get{return _result;}}
}
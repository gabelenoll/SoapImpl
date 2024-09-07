using System.Xml.Linq;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed class GetPartnerCompaniesRequestMessage : WFHelperRequestMessage<object?>
{
    public GetPartnerCompaniesRequestMessage() :
        base("GetPartnerCompanies")
    {
    }

    protected override void FillBody(XElement body, object? _)
    {
    }
}
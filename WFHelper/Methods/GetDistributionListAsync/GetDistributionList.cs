using System.Threading.Tasks;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed partial class WFHelperSoapService : SOAP.ServiceBase
{
    public async Task<GetDistributionListResponseMessage> GetDistributionListAsync(string partnerCompany, string language)
    {
        return await SendMessageAsync<GetDistributionListResponseMessage, object?>(
            new GetDistributionListRequestMessage(partnerCompany, language),
            null
            );
    }
}
using System.Threading.Tasks;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed partial class WFHelperSoapService : SOAP.ServiceBase
{
    public async Task<GetPartnerCompaniesResponseMessage> GetPartnerCompaniesAsync()
    {
        return await SendMessageAsync<GetPartnerCompaniesResponseMessage, object?>(
            new GetPartnerCompaniesRequestMessage(),
            null
            );
    }
}
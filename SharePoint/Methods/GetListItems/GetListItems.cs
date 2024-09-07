using System.Collections;
using System.Threading.Tasks;
using Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed partial class WFHelperSharePointSoapService : SOAP.ServiceBase
{
    public async Task<GetListItemsResponseMessage> GetListItemsAsync(string listName)
    {
        return await SendMessageAsync<GetListItemsResponseMessage, string>(
            new GetListItemsRequestMessage(),
            listName
            );
    }
}
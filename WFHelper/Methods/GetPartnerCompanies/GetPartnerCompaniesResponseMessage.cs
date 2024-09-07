using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using MODEL = Voith.WF.OrderReleaseVP.Business.Model;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed class GetPartnerCompaniesResponseMessage : WFHelperResponseMessage<IEnumerable<string>>
{
    public override void Read(Stream stream, SOAP.IServiceRequestMessage message)
    {
        XPathNavigator nav = GetNavigator("porvp", "GetPartnerCompaniesResponse", stream);
        XPathNodeIterator it = nav.Select("porvp:pPartners/porvp:string", XMethods.NamespaceManager);
        SetResult(GetStringItems(it));
    }
}
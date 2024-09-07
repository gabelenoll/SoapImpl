using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed class GetListItemsResponseMessage : WFHelperResponseMessage<IEnumerable<string>>
{
    public override void Read(Stream stream, SOAP.IServiceRequestMessage message)
    {
        XPathNavigator nav = GetNavigator("psp", "GetListItemsResponse", stream);
        XPathNodeIterator it = nav.Select("psp:GetListItemsResult/psp:listitems/sprs:data/sprss:row/@ows_Title", XMethods.NamespaceManager);
        SetResult(GetStringItems(it));
    }
}
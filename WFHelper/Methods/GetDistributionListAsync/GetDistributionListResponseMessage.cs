using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using MODEL = Voith.WF.OrderReleaseVP.Business.Model;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed class GetDistributionListResponseMessage : WFHelperResponseMessage<IEnumerable<MODEL.DistributionListItem>>
{
    private IEnumerable<MODEL.DistributionListItem> GetItems(XPathNodeIterator it)
    {
        XmlNamespaceManager namespaceManager = XMethods.NamespaceManager;
        while(it.MoveNext())
        {
            MODEL.DistributionListItem? item = null;

            XPathNavigator? nav2 = it.Current;
            if(nav2 != null)
            {
                string? name = nav2.SelectSingleNode("porvp:Name", namespaceManager)?.Value;
                if(name != null)
                {
                    item = new MODEL.DistributionListItem(name);
                    item.Category = nav2.SelectSingleNode("porvp:Category", namespaceManager)?.Value;
                    item.SortName = nav2.SelectSingleNode("porvp:SortName", namespaceManager)?.Value;
                    item.SortCategory = nav2.SelectSingleNode("porvp:SortCategory", namespaceManager)?.Value;
                }
            }

            if(item != null)
                yield return item;
        }
    }

    public override void Read(Stream stream, SOAP.IServiceRequestMessage message)
    {
        XPathNavigator nav = GetNavigator("porvp", "GetDistributionListResponse", stream);
        XPathNodeIterator it = nav.Select("porvp:pData/porvp:DistributionListItemData", XMethods.NamespaceManager);
        SetResult(GetItems(it));
    }
}
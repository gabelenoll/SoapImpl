using System.Xml.Linq;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed class GetListItemsRequestMessage : WFHelperRequestMessage<string>
{
    public GetListItemsRequestMessage() :
        base("GetListItems")
    {
    }

    protected override void FillBody(XElement body, string listName)
    {
        XElement e1 = new XElement(XnPayloadSharePoint + Name);
        XElement e2 = new XElement(XnPayloadSharePoint + "listName");
        e2.Add(listName);
        e1.Add(e2);
        body.Add(e1);
    }
}
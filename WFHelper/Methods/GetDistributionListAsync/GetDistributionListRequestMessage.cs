using System.Xml.Linq;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal sealed class GetDistributionListRequestMessage : WFHelperRequestMessage<object?>
{
    private string _partnerCompany;
    private string _language;

    public GetDistributionListRequestMessage(string partnerCompany, string language) :
        base("GetDistributionList")
    {
        _partnerCompany = partnerCompany;
        _language = language;
    }

    protected override void FillBody(XElement body, object? _)
    {
        XElement e1 = new XElement(XnPayloadOrvp + Name);
        XElement e2 = new XElement(XnPayloadOrvp + "pPartnerCompany");
        e2.Add(_partnerCompany);
        e1.Add(e2);
        XElement e3 = new XElement(XnPayloadOrvp + "pLanguage");
        e3.Add(_language);
        e1.Add(e3);
        body.Add(e1);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using SOAP = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.SOAP11;

namespace Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;

internal abstract class WFHelperResponseMessage<TResult> : SOAP.ResponseMessage<TResult> where TResult: notnull
{
    protected XPathNavigator GetNavigator(string prefix, string name, Stream stream)
    {
        return XMethods.ReturnNavigator(
            XDocument.Load(stream).CreateNavigator(XMethods.NameTable).SelectSingleNode(
                String.Format("/s:Envelope/s:Body/{0}:{1}", prefix, name),
                XMethods.NamespaceManager
                ));
    }

    protected IEnumerable<string> GetStringItems(XPathNodeIterator it)
    {
        while(it.MoveNext())
        {
            XPathNavigator? nav2 = it.Current;
            if(nav2 != null)
                yield return nav2.Value;
        }
    }
}
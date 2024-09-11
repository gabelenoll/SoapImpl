using System;

namespace Voith.WF.OrderReleaseVP.Business.Controllers;

public sealed class WFHelperControllerException : Exception
{
    public static WFHelperControllerException UnexpectedNullValueFromService
    {
        get
        {
            return new WFHelperControllerException("Service unexpectedly return a null value.");
        }
    }

    public WFHelperControllerException(string message) :
        base(message)
    {
    }
}
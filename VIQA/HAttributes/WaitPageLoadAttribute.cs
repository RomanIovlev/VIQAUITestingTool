using System;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class WaitPageLoadAttribute : Attribute
    {
    }
}


using System;
using NUnit.Framework;
using VIQA.Common.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.Common
{
    public class DefaultAllert : IAlerting
    {
        public Exception ThrowError(string errorMsg)
        {
            VISite.Logger.Error(errorMsg);
            Assert.Fail(errorMsg);
            return new Exception(errorMsg);
        }
    }
}
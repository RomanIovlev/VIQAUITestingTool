using System;
using NUnit.Framework;
using VIQA.Common.Interfaces;

namespace VIQA.Common
{
    public class DefaultAllert : IAlerting
    {
        private readonly ILogger _logger;

        public DefaultAllert(ILogger logger)
        {
            _logger = logger;
        }

        public Exception ThrowError(string errorMsg)
        {
            _logger.Error(errorMsg);
            Assert.Fail(errorMsg);
            return new Exception(errorMsg);
        }
    }
}

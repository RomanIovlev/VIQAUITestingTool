using System;

namespace VIQA.Common.Interfaces
{
    public interface IAlerting
    {
        Exception ThrowError(string errorMsg);
    }
}

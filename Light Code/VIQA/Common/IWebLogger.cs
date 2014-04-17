using System;

namespace VIQA.Common
{
    interface IWebLogger
    {
        void Event(string msg);
        void HideLogging(Action action);
    }
}

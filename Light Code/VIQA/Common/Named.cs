using System;

namespace VIQA.Common
{
    public class Named
    {
        private string _name;

        protected Func<string> DefaultNameFunc = () => "";

        public string Name
        {
            set { _name = value; }
            get { return _name ?? DefaultNameFunc.Invoke(); }
        }
    }
}

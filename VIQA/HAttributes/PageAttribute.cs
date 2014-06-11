using System;
using System.Reflection;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class PageAttribute : Attribute
    {
        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                _checkUrl = PageCheckType.Contains;
            }
        }

        public bool IsHomePage { get; set; }

        private string _title;
        public string Title { 
            get { return _title; }
            set
            {
                _title = value;
                _checkTitle = PageCheckType.Contains;
        } }

        public bool IsCheckUrlSetManual { get { return _isCheckUrlSetManual; } }
        public bool IsCheckTitleSetManual { get { return _isCheckTitleSetManual; } }

        private bool _isCheckUrlSetManual;
        private bool _isCheckTitleSetManual;
        private PageCheckType _checkUrl = PageCheckType.NoCheck;
        private PageCheckType _checkTitle = PageCheckType.NoCheck;

        public PageCheckType CheckUrl { get { return _checkUrl; } set { _checkUrl = value;
        _isCheckUrlSetManual = true;
        } }
        public PageCheckType CheckTitle { get { return _checkTitle; } set { _checkTitle = value;
        _isCheckTitleSetManual = true;
        } }
        
        public static PageAttribute Handler(FieldInfo field)
        {
            return field.GetCustomAttribute<PageAttribute>(false);
        }

        public static PageAttribute Handler(Object obj)
        {
            return obj.GetType().GetCustomAttribute<PageAttribute>(false);
        }
    }

    public enum PageCheckType { NoCheck, Equal, Contains }
}

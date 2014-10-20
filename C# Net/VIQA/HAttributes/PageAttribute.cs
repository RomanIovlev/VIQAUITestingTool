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
                _urlCheckType = PageCheckType.Contains;
            }
        }

        public bool IsHomePage { get; set; }

        private string _title;
        public string Title { 
            get { return _title; }
            set
            {
                _title = value;
                _titleCheckType = PageCheckType.Contains;
        } }

        public bool IsCheckUrlSetManual { get { return _isCheckUrlSetManual; } }
        public bool IsCheckTitleSetManual { get { return _isCheckTitleSetManual; } }

        private bool _isCheckUrlSetManual;
        private bool _isCheckTitleSetManual;
        private PageCheckType _urlCheckType = PageCheckType.NoCheck;
        private PageCheckType _titleCheckType = PageCheckType.NoCheck;

        public PageCheckType UrlCheckType { get { return _urlCheckType; } set { _urlCheckType = value;
        _isCheckUrlSetManual = true;
        } }
        public PageCheckType TitleCheckType { get { return _titleCheckType; } set { _titleCheckType = value;
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

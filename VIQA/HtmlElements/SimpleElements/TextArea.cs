using OpenQA.Selenium;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class TextArea : TextElement, ITextArea
    {
        public const string LocatorTmpl = "textarea[{0}={1}]";
        public static string CommonLocatorById(string id) { return string.Format(LocatorTmpl, "id", id); }
        public static string CommonLocatorByNamed(string id) { return string.Format(LocatorTmpl, "name", id); }
        public static string CommonLocatorByClassName(string id) { return string.Format(LocatorTmpl, "class", id); }

        protected override string _typeName { get { return "TextArea"; } }

        public TextArea() { }
        public TextArea(string name) : base(name) { }
        public TextArea(string name, string cssSelector) : base(name, cssSelector) { }
        public TextArea(string name, By byLocator) : base(name, byLocator) { }
        public TextArea(By byLocator) : base(byLocator) { }
        public TextArea(string name, IWebElement webElement) : base(name, webElement) { }
        public TextArea(IWebElement webElement) : base(webElement) { }
        
        public void Input(string text)
        {
            DoVIAction("Input text " + text + " in text area", () => GetWebElement().SendKeys(text));
        }

        public void NewInput(string text)
        {
            Clear();
            Input(text);
        }

        public void Clear()
        {
            DoVIAction("Clear text area", () => GetWebElement().Clear());
        }

        public void SetValue<T>(T value)
        {
            if (value == null) return;
            NewInput(value.ToString());
        }

    }
}


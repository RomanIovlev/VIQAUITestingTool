using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements.BaseClasses
{
    public class VIList<T> : VIElement, IVIList<T> where T : VIElement
    {
        public Func<T> CreateElementFunc = () => (T)Activator.CreateInstance(typeof(T));
        
        public Dictionary<string, T> Elements = new Dictionary<string, T>();
        
        public By GetLocator(string value)
        {
            var locatorTemplate = CreateElementFunc().Locator;
            return locatorTemplate.FillByTemplate(value);
        }
        
        public T GetVIElementByName(string value)
        {
            if (!Elements.ContainsKey(value))
            {
                var viElement = CreateElementFunc();
                viElement.Locator = GetLocator(value);
                Elements.Add(value, (T)viElement.GetVIElement());
            }
            return Elements[value];
        }

        #region Constructors

        public VIList() { }
        public VIList(string name) : base(name) { }
        public VIList(string name, By rootCssSelector, Func<T> selectorTemplate) : this(name)
        {
            Locator = rootCssSelector;
            CreateElementFunc = selectorTemplate;
        }

        public VIList(string name, string cssSelector) : base(name)
        {
            CreateElementFunc = () => (T)Activator.CreateInstance(typeof(T), cssSelector);
        }
        public VIList(string name, By bySelector) : base(name)
        {
            CreateElementFunc = () => (T)Activator.CreateInstance(typeof(T), bySelector);
        }

        public VIList(string name, Func<T> selectorTemplate) : this(name, null, selectorTemplate) { }
        #endregion

        public List<string> ListOfValues;
        public Func<ISearchContext, Dictionary<string,T>> GetAllElementsFunc;
        
        public Dictionary<string, T> GetAllElements()
        {
            try
            {
                var context = CheckPreconditionsAndGetContext();
                Elements = GetAllElementsFunc(context).ToDictionary(el => el.Key, el =>
                {
                    if (string.IsNullOrEmpty(el.Value.Name))
                        el.Value.Name = FullName + " with value " + el.Key;
                    return el.Value;
                });
            }
            catch (Exception) { throw VISite.Alerting.ThrowError("GetListOfValuesFunc not set for " + Name); }

            ListOfValues = Elements.Select(el => el.Key).ToList();
            return Elements;
        }

        private ISearchContext CheckPreconditionsAndGetContext()
        {
            if (GetAllElementsFunc == null)
                throw VISite.Alerting.ThrowError("GetListOfValuesFunc not set for " + Name);
            ISearchContext context;
            if (Context == null)
                return WebDriver;
            var countOfElements = WebDriver.FindElements(Context).Count;
            if (countOfElements == 1)
                return WebDriver.FindElements(Context).First();
            throw VISite.Alerting.ThrowError(Name + "Find elements by Context failed. Found " + countOfElements + " elements");
        }

    }
}

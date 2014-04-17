using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using VIQA.Common;
using VIQA.HAttributes;
using VIQA.HtmlElements.Interfaces;
using VIQA.SiteClasses;

namespace VIQA.HtmlElements
{
    public class VIElementsList : Named
    {
        protected By _locator;
        public By Context;
        
        private void SetViElement(FieldInfo viElement)
        {
            var instance = (VIElement) viElement.GetValue(this) ??
                (VIElement)Activator.CreateInstance(viElement.FieldType);
            var name = NameAttribute.GetName(viElement);
            if (!string.IsNullOrEmpty(name))
                instance.Name = name;
            var locator = LocateAttribute.GetLocator(viElement);
            if (locator != null)
                instance.Locator = locator;
            if (_locator != null)
                instance.Context = (Context != null)
                    ? new ByChained(Context, _locator)
                    : _locator;
            var clickReloadsPage = viElement.GetCustomAttribute<ClickReloadsPageAttribute>(false);
            if (clickReloadsPage != null)
                instance.WithPageLoadAction = true;
            viElement.SetValue(this, instance);
        }

        public List<FieldInfo> GetElements() { return GetElements<IVIElement>(); }

        public List<FieldInfo> GetElements<T>()
        {
            return
                GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(_ => typeof (T).IsAssignableFrom(_.FieldType)).ToList();
        }

        public Func<Object, Object> FillRule { set; get; }

        protected static Func<Object, Object> ToFillRule<T>(Func<T, Object> typeFillRule)
        {
            return o => new Func<T, object>(typeFillRule).Invoke((T) o);
        }

        private Dictionary<string, ISetValue> _setValueElements;
        public Dictionary<string, ISetValue> SetValueElements
        {
            get { return _setValueElements ?? (_setValueElements = 
                GetElements<ISetValue>().Select(_ => (ISetValue)_.GetValue(this)).ToDictionary(_ => _.Name, _ => _)); }
        }

        public void FillElements(Dictionary<ISetValue, Object> values)
        {
            FillElements(values.ToDictionary(_ => _.Key.Name, _ => _.Value));
        }

        public void FillElements(Dictionary<string, Object> values)
        {
            if (values.Keys.All(SetValueElements.ContainsKey))
                values.Where(_ => _.Value != null).ForEach(pair => SetValueElements[pair.Key].SetValue(pair.Value));
            else
                throw VISite.Alerting.ThrowError("Unknown Keys for Data form.".LineBreak() +
                    "Possible:" + SetValueElements.Keys.Print().LineBreak() +
                    "Requested:" + values.Keys.Print());
        }
        
        public void FillForm(Object data)
        {
            SetValueElements.Select(_ => _.Value).Where(_ => _.FillRule != null)
                .ForEach(element =>
                {
                    try { element.SetValue(element.FillRule(data)); }
                    catch { }
                });
        }

        public void FillElement(string name, string value)
        {
            SetValueElements[name].SetValue(value);
        }

        public VIElementsList()
        {
            GetElements().ForEach(SetViElement);
        }
    }
}

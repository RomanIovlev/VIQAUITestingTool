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
    public class VIElementsSet : Named
    {
        protected By _locator;
        public By Context;
        private VISite _site;
        public bool IsSiteSet { get { return _site != null; } }
        public VISite Site { get { return _site = _site ?? (DefaultSite ?? new VISite()); } set { _site = value; } }
        public static VISite DefaultSite { set; get; }
        
        private void SetViElement(FieldInfo viElement)
        {
            var instance = (VIElement) viElement.GetValue(this) ??
                (VIElement)Activator.CreateInstance(InterfacesMap(viElement.FieldType));
            instance.Site = Site;
            var name = NameAttribute.GetName(viElement);
            if (!string.IsNullOrEmpty(name))
                instance.Name = name;
            var locator = LocateAttribute.GetLocator(viElement);
            if (locator != null)
                instance.Locator = locator;
            var fillFromName = FillFromFieldAttribute.GetFieldName(viElement);
            if (!string.IsNullOrEmpty(fillFromName))
                instance.FillRule = data => data.GetFieldByName(fillFromName);
            if (_locator != null)
                instance.Context = (Context != null)
                    ? new ByChained(Context, _locator)
                    : _locator;
            var clickableElement = instance as IClickable;
            if (clickableElement != null)
            {
                var clickReloadsPage = ClickOpensPageAttribute.Handler(viElement);
                if (!string.IsNullOrEmpty(clickReloadsPage))
                    clickableElement.ClickOpensPage = clickReloadsPage;
                viElement.SetValue(this, clickableElement);
                ((VIElement)clickableElement).InitSubElements();
                return;
            }
            viElement.SetValue(this, instance);
            instance.InitSubElements();
        }

        private static Type InterfacesMap(Type fieldType)
        {
            if (!fieldType.IsInterface) return fieldType;
            var listOfTypes = VIElement.InterfaceTypeMap.Where(el => fieldType == el.Key).ToList();
            if (listOfTypes.Count() == 1)
                return listOfTypes.First().Value;
            VISite.Alerting.ThrowError("Unknown interface: " + fieldType);
            return fieldType;
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

        private Dictionary<string, IHaveValue> _withValueElements;
        public Dictionary<string, IHaveValue> WithValueElements
        {
            get { return _withValueElements ?? (_withValueElements = 
                GetElements<IHaveValue>().Select(_ => (IHaveValue)_.GetValue(this)).ToDictionary(_ => _.Name, _ => _)); }
        }

        public void FillElements(Dictionary<IHaveValue, Object> values)
        {
            FillElements(values.ToDictionary(_ => _.Key.Name, _ => _.Value));
        }

        private string ObjToString(Object obj)
        {
            var objects = obj as IEnumerable<object>;
            return objects != null 
                ? string.Join(", ", objects.Select(el => el.ToString())) 
                : obj.ToString();
        }

        public void FillElements(Dictionary<string, Object> values)
        {
            var vals = values.ToDictionary(_ => _.Key, _ => ObjToString(_.Value));
            VISite.Logger.Event("Fill elements: '" + Name + "'".LineBreak() + "With data: " + vals.Print());
            if (values.Keys.All(WithValueElements.ContainsKey))
                try
                { values.Where(_ => _.Value != null).ForEach(pair => WithValueElements[pair.Key].SetValue(pair.Value)); }
                catch (Exception ex) { VISite.Alerting.ThrowError("Error in FillElements. Exception: " + ex); }
            else
                throw VISite.Alerting.ThrowError("Unknown Keys for Data form.".LineBreak() +
                    "Possible:" + WithValueElements.Keys.Print().LineBreak() +
                    "Requested:" + values.Keys.Print());
        }
        
        public void FillElements(Object data)
        {
            VISite.Logger.Event("Fill elements: '" + Name + "'".LineBreak() + "With data: " + data);
            WithValueElements.Select(_ => _.Value).Where(_ => _.FillRule != null)
                .ForEach(element =>
                {
                    try { element.SetValue(element.FillRule(data)); }
                    catch (Exception ex) { VISite.Alerting.ThrowError("Error in FillElements. Exception: " + ex); }
                });
        }

        public bool CompareValuesWith(Object data, Func<string, string, bool> compareFunc = null)
        {
            VISite.Logger.Event("Check Form values: '" + Name + "'".LineBreak() + "With data: " + data);
            var result = true;
            var CompareFunc = compareFunc ?? VIElement.DefaultCompareFunc;
            var elements = WithValueElements.Select(_ => _.Value).Where(_ => _.FillRule != null);
            foreach (var element in elements) {
                try
                {
                    var expected = element.FillRule(data);
                    var expectedEnum = expected as IEnumerable<Object>;
                    if (expectedEnum == null)
                    {
                        if (CompareFunc(element.Value, expected.ToString()))
                            continue;
                    }
                    else
                    {
                        var expecctedList = expectedEnum.ToList();
                        if (expecctedList.Count(el => element.Value.Contains(el.ToString())) == expecctedList.Count())
                        continue;
                    }
                    result = false;
                    break;
                }
                catch (Exception ex) { VISite.Alerting.ThrowError("Error in CompareValuesWith. Exception: " + ex); }
            }
            return result;
        }

        public void FillElement(string name, string value)
        {
            WithValueElements[name].SetValue(value);
        }

        public void InitSubElements()
        {
            GetElements().ForEach(SetViElement);
        }
    }
}

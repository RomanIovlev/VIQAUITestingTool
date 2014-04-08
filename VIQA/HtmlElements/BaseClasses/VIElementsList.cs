using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using VIQA.Common;
using VIQA.HAttributes;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements.BaseClasses
{
    public class VIElementsList : Named
    {
        protected By _locator;
        public By Context;

        private void SetViElement(FieldInfo viElement)
        {
            var type = (!viElement.FieldType.IsAbstract)
                ? viElement.FieldType
                : viElement.GetValue(this).GetType();
            var instance = (VIElement)Activator.CreateInstance(type);
            var name = NameAttribute.GetName(viElement);
            if (name != null)
                instance.Name = name;
            var locator = LocateAttribute.GetLocator(viElement);
            if (locator != null)
                instance.Locator = locator;
            if (_locator != null)
                instance.Context = (Context != null)
                    ? new ByChained(Context, _locator)
                    : _locator;
            viElement.SetValue(this, instance);
        }

        public VIElementsList()
        {
            var viElements = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(_ => typeof(IVIElement).IsAssignableFrom(_.FieldType));
            viElements.ForEach(SetViElement);
        }
    }
}

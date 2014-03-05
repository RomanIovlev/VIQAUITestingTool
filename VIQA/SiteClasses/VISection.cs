using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using VIQA.Common;
using VIQA.HAttributes;
using VIQA.HtmlElements;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.SiteClasses
{
    public class VISection : Named
    {
        public List<By> Context { get; set; }
        
        private void ActivateSections()
        {
            var viSections = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(_ => typeof(VISection).IsAssignableFrom(_.FieldType)).ToList();
            Func<FieldInfo, VISection, VISection> initFunc = (field, vs) => (this is VIPage)
                ? vs.InitFromAttr(field, null)
                : vs.InitFromAttr(field, this);
            viSections.ForEach(viSection =>
                viSection.SetValue(this, initFunc.Invoke(viSection, (VISection)Activator.CreateInstance(viSection.FieldType))));
        }

        private void ActivateElements()
        {
            var viElements = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(_ => typeof(IVIElement).IsAssignableFrom(_.FieldType));
            viElements.ForEach(viElement => VIElement.Activate(this, viElement));
        }
        
        protected VISection InitFromAttr(FieldInfo sectionField, VISection parentSection)
        {
            var nameAttr = sectionField.GetCustomAttribute<NameAttribute>(false) ?? GetType().GetCustomAttribute<NameAttribute>(false);
            if (nameAttr != null)
                Name = nameAttr.Name;
            if (parentSection != null)
                Name = parentSection.Name + "." + Name;

            Context = new List<By>();
            if (parentSection != null)
                Context.AddRange(parentSection.Context);
            var context = LocateAttribute.GetLocator(sectionField);
            if (context != null)
                Context.Add(context);

            ActivateSections();
            ActivateElements();
            return this;
        }

    }
}

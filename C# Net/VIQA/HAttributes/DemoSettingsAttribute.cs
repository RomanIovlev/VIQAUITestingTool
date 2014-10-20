using System;
using System.Reflection;
using VIQA.SiteClasses;

namespace VIQA.HAttributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class DemoSettingsAttribute : Attribute
    {
        public string BgColor = "yellow";
        public string FrameColor = "red";
        public int TimeoutInSec = 1;

        public static HighlightSettings Get(FieldInfo field)
        {
            var demoSettings = field.GetCustomAttribute<DemoSettingsAttribute>(false);
            return (demoSettings != null)
                ? new HighlightSettings(demoSettings.BgColor, demoSettings.FrameColor, demoSettings.TimeoutInSec)
                : new HighlightSettings();
        }

        public static HighlightSettings Get(Object obj)
        {
            var demoSettings = obj.GetType().GetCustomAttribute<DemoSettingsAttribute>(false);
            return demoSettings != null 
                ? new HighlightSettings(demoSettings.BgColor, demoSettings.FrameColor, demoSettings.TimeoutInSec) 
                : new HighlightSettings();
        }
    }
}

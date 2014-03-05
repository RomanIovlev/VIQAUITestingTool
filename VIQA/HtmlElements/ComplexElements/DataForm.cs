using System;
using System.Collections.Generic;
using System.Linq;
using VIQA.Common;
using VIQA.Common.Interfaces;
using VIQA.HtmlElements.Interfaces;

namespace VIQA.HtmlElements
{
    public class DataForm
    {
        public string Name { set; get; }
        public readonly Dictionary<string, ISetValue> Elements;
        private IAlerting _alerting;

        public IAlerting Alerting
        {
            get
            {
                if (_alerting != null)
                    return _alerting;
                throw new Exception("Alerting not specified");
            }
            set
            {
                _alerting = value;
            }
        }

        public DataForm() { }
        public DataForm(string name, List<ISetValue> elements)
        {
            Name = name;
            if (elements.Any())
                try { Alerting = elements.First().Alerting; } catch  { }
            if (elements.GroupBy(_ => _.Name).Any(_ => _.Count() > 1))
                throw Alerting.ThrowError(string.Format("Duplicate keys ({0}) in {1}", elements.GroupBy(_ => _.Name).Where(_ => _.Count() > 1).Select(_ => _.Key).Print(), Name));
            Elements = elements.ToDictionary(el => el.Name, el => el);
        }

        public void FillForm(Dictionary<string, Object> values)
        {
            if (values.Keys.All(key => Elements.Keys.Contains(key)))
                values.Where(_ => _.Value != null).ForEach(pair => Elements[pair.Key].SetValue(pair.Value));
            else 
                throw Alerting.ThrowError("Unknown Keys for Data form.".LineBreak() + 
                    "Possible:" + Elements.Keys.Print().LineBreak() + 
                    "Requested:" + values.Keys.Print());
        }

        public void FillElement(string name, string value)
        {
            Elements[name].SetValue(value);
        }
    }

    
    public class DataForm<T> : DataForm
    {
        private readonly Dictionary<string, Func<T, Object>> _fillingRules;

        public DataForm() { }

        public DataForm(string name, Dictionary<ISetValue, Func<T, Object>> elements) : base(name, elements.Keys.ToList())
        {
            if (elements.GroupBy(_ => _.Key.Name).Any(_ => _.Count() > 1))
                throw Alerting.ThrowError("Duplicate keys in " + Name);
            _fillingRules = elements.Where(_=> _.Value != null).ToDictionary(_ => _.Key.Name, _=> _.Value);
        }

        public void FillForm(T data)
        {
            FillForm(_fillingRules.Where(_ => (_.Key != null) && (_.Value.Invoke(data) != null)).ToDictionary(_ => _.Key, _ => _.Value.Invoke(data)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Model
{
    public class BaseModel
    {
        public readonly IDictionary<string, string> ModelProperties;

        public BaseModel()
        {
            this.ModelProperties = new Dictionary<string, string>();
        }

        public IList<string> GetHeaders()
        {
            return this.GetType().GetFields().Select(f => f.Name).ToList();
        }

        public T GetPropertyValue<T>(string propertyName)
        {
            return (T)(this.GetType().GetField(propertyName,
                BindingFlags.Public | BindingFlags.Instance).GetValue(this));
        }

        public void SetProperty(string key, string value)
        {
            if (!this.ModelProperties.ContainsKey(key))
            {
                this.ModelProperties[key] = value;
            }
            else
            {
                this.ModelProperties.Add(key, value);
            }
        }
    }
}

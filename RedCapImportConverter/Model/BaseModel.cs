using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Model
{
    public abstract class BaseModel
    {
        public IList<string> GetHeaders()
        {
            return this.GetType().GetFields().Select(f => f.Name).ToList();
        }

        public T GetPropertyValue<T>(string propertyName)
        {
            return (T)(this.GetType().GetField(propertyName,
                BindingFlags.Public | BindingFlags.Instance).GetValue(this));
        }
    }
}

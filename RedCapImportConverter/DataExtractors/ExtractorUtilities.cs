using RedCapImportConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.DataExtractors
{
    public static class ExtractorUtilities
    {
        public static void UpdateProperty(BaseModel model, string propertyName, string value)
        {
            Type modelType = model.GetType();
            FieldInfo property = modelType.GetField(propertyName,
                BindingFlags.Public | BindingFlags.Instance);

            property.SetValue(model, value);
        }

        public static void UpdateMapping(BaseModel model, string key, string value)
        {
            model.SetProperty(key, value);
        }
    }
}

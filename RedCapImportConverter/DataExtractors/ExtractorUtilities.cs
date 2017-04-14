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
        public static void UpdatePropertyName(IModelObject model, string propertyNameKey, string value)
        {
            Type modelType = model.GetType();
            FieldInfo property = modelType.GetField(PropertyNameLookup[propertyNameKey],
                BindingFlags.Public | BindingFlags.Instance);

            property.SetValue(model, value);
        }

        private static readonly IDictionary<string, string> PropertyNameLookup = new Dictionary<string, string>()
        {
            {"allaveragediastolic", "avall_dia_abp" },
            {"allstddiastolic", "stdall_dia_abp" },
            {"dayaveragesystolic", "avday_sys_abp" }
        };
    }
}

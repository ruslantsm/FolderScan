using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace WordEntryDal.Extension
{
	public static class DataTableExtensions
	{
		public static DataTable ToDataTable<T>(this IEnumerable<T> source)
		{
			DataTable table = new DataTable();

			var binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
			var options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

			var properties = ReflectionExtensions.GetProperties<T>(binding, options).ToList();

			foreach (var property in properties)
			{
				table.Columns.Add(property.Name, property.PropertyType);
			}

			object[] values = new object[properties.Count];

			foreach (T item in source)
			{
				for (int i = 0; i < properties.Count; i++)
				{
					values[i] = properties[i].GetValue(item, null);
				}

				table.Rows.Add(values);
			}

			return table;
		}
	}
}

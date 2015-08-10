using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace WordEntryDal.Extension
{
	public static class DbContextExtensions
	{
		public static int ExecuteTableValueProcedure<T>(this DbContext context, IEnumerable<T> data,
								string procedureName, string tableParamName, string tableTypeName)
		{
			DataTable table = data.ToDataTable();

			SqlParameter parameter = new SqlParameter(tableParamName, table);
			parameter.SqlDbType = SqlDbType.Structured;
			parameter.TypeName = tableTypeName;

			string sql = String.Format("EXEC {0} {1};", procedureName, tableParamName);

			return context.Database.ExecuteSqlCommand(sql, parameter);
		}
	}
}

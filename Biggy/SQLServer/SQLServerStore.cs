﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biggy.SQLServer
{
  public class SQLServerStore<T> : BiggyRelationalStore<T> where T : new()
  {
    public SQLServerStore(DbHost dbHost) : base(dbHost) { }
    public SQLServerStore(string connectionString) : base(new SQLServerHost(connectionString)) { }

    public override string GetInsertReturnValueSQL(string delimitedPkColumn) {
      return string.Format("; SELECT SCOPE_IDENTITY() as {0}", delimitedPkColumn);
    }

    public override string BuildSelect(string where, string orderBy, int limit) {
      string sql = limit > 0 ? "SELECT TOP " + limit + " {0} FROM {1} " : "SELECT {0} FROM {1} ";
      if (!string.IsNullOrEmpty(where)) {
        sql += where.Trim().StartsWith("where", StringComparison.OrdinalIgnoreCase) ? where : " WHERE " + where;
      }
      if (!String.IsNullOrEmpty(orderBy)) {
        sql += orderBy.Trim().StartsWith("order by", StringComparison.OrdinalIgnoreCase) ? orderBy : " ORDER BY " + orderBy;
      }
      return sql;
    }


    public override string GetSingleSelect(string delimitedTableName, string where) {
      return string.Format("SELECT TOP 2 * FROM {0} WHERE {1}", delimitedTableName, where);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Threading;

namespace Transaction.Dapper
{
    public class TransactionDapper : DBConnection
    {
        public TransactionDapper(IDbConnection connection)
            : base(connection)
        {
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.Connection.Execute(sql, param, this.Transaction, commandTimeout, commandType);
        }
    }
}

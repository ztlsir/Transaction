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
    public class DapperConnection
    {
        private IDbConnection connection = null;

        private IDbTransaction transaction = null;

        public DapperConnection(string connStr)
        {
            var dapperTransactionManager = TransactionManagerFactory.GetOrSetTransactionManager(() =>
            {
                return new DapperTransactionManager(new SqlConnection(connStr));
            }) as DapperTransactionManager;

            this.connection = dapperTransactionManager.connectionHolder.Connection;
            this.transaction = dapperTransactionManager.connectionHolder.Transaction;
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.connection.Execute(sql, param, this.transaction, commandTimeout, commandType);
        }
    }
}

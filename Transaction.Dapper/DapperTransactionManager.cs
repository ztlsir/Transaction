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
    public class DapperTransactionManager : BaseTransactionManager
    {
        public ConnectionHolder connectionHolder { get; private set; }

        public DapperTransactionManager(IDbConnection connection)
        {
            connectionHolder = new ConnectionHolder(connection);
        }

        public override void BeginTransaction()
        {
            if (!this.IsBeginTransaction())
            {
                this.connectionHolder.Connection.Open();
                this.connectionHolder.Transaction = this.connectionHolder.Connection.BeginTransaction();
            }
        }

        private bool IsBeginTransaction()
        {
            return this.connectionHolder.Transaction != null;
        }

        public override void Commit()
        {
            if (this.IsBeginTransaction())
                this.connectionHolder.Transaction.Commit();
        }

        public override void Rollback()
        {
            if (this.IsBeginTransaction())
                this.connectionHolder.Transaction.Rollback();
        }

        public override void Dispose()
        {
            if (this.IsBeginTransaction())
            {
                this.connectionHolder.Transaction.Dispose();
                this.connectionHolder.Transaction = null;
            }

            if (this.connectionHolder.Connection != null)
            {
                this.connectionHolder.Connection.Dispose();
                this.connectionHolder.Connection = null;
            }
        }
    }
}

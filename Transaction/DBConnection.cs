using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace Transaction
{
    public class DBConnection
    {

        [ThreadStatic]
        private IDbConnection connection = null;

        [ThreadStatic]
        private IDbTransaction transaction = null;

        public IDbConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }

        public DBConnection(IDbConnection connection)
        {
            this.connection = connection;
        }

        private bool IsBeginTransaction()
        {
            return this.transaction != null;
        }

        public void BeginTransaction()
        {
            if (!this.IsBeginTransaction())
            {
                this.Connection.Open();
                this.transaction = this.Connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (this.IsBeginTransaction())
                this.transaction.Commit();
        }

        public void Rollback()
        {
            if (this.IsBeginTransaction())
                this.transaction.Rollback();
        }

        public void Dispose()
        {
            if (this.IsBeginTransaction())
            {
                this.Transaction.Dispose();
                this.transaction = null;
            }

            if (this.connection != null)
            {
                this.Connection.Dispose();
                this.connection = null;
            }
        }
    }
}

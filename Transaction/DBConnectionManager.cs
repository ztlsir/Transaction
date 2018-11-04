using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transaction
{
    public abstract class DBConnectionManager
    {
        private const string CONNECTION_SLOT = "CONNECTION_SLOT";

        public static TDBConnection GetDBConnection<TDBConnectionManager, TDBConnection>(string connStr)
            where TDBConnectionManager : DBConnectionManager, new()
            where TDBConnection : DBConnection
        {
            var connectionManager = new TDBConnectionManager();
            var connection = connectionManager.GetOrCreateDBConnection<TDBConnection>(connStr);

            if (TransactionManager.IsUseTransaction())
            {
                connection.BeginTransaction();
            }

            return connection;
        }

        private TDBConnection GetOrCreateDBConnection<TDBConnection>(string connStr)
            where TDBConnection : DBConnection
        {
            var connection = GetCurrentThreadDBConnection<TDBConnection>();

            if (connection != null)
            {
                return connection;
            }

            connection = CreateDBConnection<TDBConnection>(connStr);

            return connection;
        }

        public static TDBConnection GetCurrentThreadDBConnection<TDBConnection>()
            where TDBConnection : DBConnection
        {
            var connDataSlot = Thread.GetNamedDataSlot(CONNECTION_SLOT);
            if (connDataSlot == null)
            {
                return null;
            }

            return Thread.GetData(connDataSlot) as TDBConnection;
        }

        private TResult CreateDBConnection<TResult>(string connStr)
            where TResult : DBConnection
        {
            var connection = (TResult)Activator.CreateInstance(typeof(TResult), CreateConnection(connStr));

            SetCurrentThreadDBConnection(connection);

            return connection;
        }

        protected abstract IDbConnection CreateConnection(string connStr);

        private static void SetCurrentThreadDBConnection(DBConnection connection)
        {
            var connDataSlot = GetOrAllocateNamedDataSlot(CONNECTION_SLOT);
            Thread.SetData(connDataSlot, connection);
        }

        public static LocalDataStoreSlot GetOrAllocateNamedDataSlot(string slotName)
        {
            var slot = Thread.GetNamedDataSlot(slotName);
            if (slot == null)
            {
                slot = Thread.AllocateNamedDataSlot(slotName);
            }

            return slot;
        }
    }
}

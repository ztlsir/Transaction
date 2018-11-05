using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace Transaction.Dapper
{
    public class ConnectionHolder
    {
        public IDbConnection Connection { get; set; }

        public IDbTransaction Transaction { get; set; }

        public ConnectionHolder(IDbConnection connection)
        {
            this.Connection = connection;
        }

    }
}

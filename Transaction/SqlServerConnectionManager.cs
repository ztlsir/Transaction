using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction
{
    public class SqlServerConnectionManager : DBConnectionManager
    {

        public SqlServerConnectionManager()
        {
        }

        protected override IDbConnection CreateConnection(string connStr)
        {
            return new SqlConnection(connStr);
        }
    }
}

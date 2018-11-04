using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction
{
    public class BaseTransactionInterceptor
    {
        protected void ExecuteByTransection(Action transactionAction)
        {
            TransactionManager.TurnOn();

            DBConnection connection = null;
            try
            {
                transactionAction();

                connection = DBConnectionManager.GetCurrentThreadDBConnection<DBConnection>();
                connection.Commit();
            }
            catch (Exception)
            {
                connection.Rollback();
            }
            finally
            {
                TransactionManager.TurnOff();
                connection.Dispose();
            }
        }
    }
}

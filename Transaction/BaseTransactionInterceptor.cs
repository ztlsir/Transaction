﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transaction
{
    public class BaseTransactionInterceptor
    {
        protected void ExecuteByTransection(Action transactionAction)
        {
            TransactionSwitch.TurnOn();

            BaseTransactionManager transactionManager = null;
            try
            {
                transactionAction();

                transactionManager = TransactionManagerFactory.GetCurrentThreadTransactionManager();
                transactionManager.Commit();
            }
            catch (Exception)
            {
                transactionManager.Rollback();
            }
            finally
            {
                TransactionSwitch.TurnOff();
                transactionManager.Dispose();
                TransactionManagerFactory.ClearCurrentThreadTransactionManager();
            }
        }
    }
}

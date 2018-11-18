using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transaction
{
    public abstract class TransactionManagerFactory
    {
        private const string TRANSACTION_MANAGER = "TRANSACTION_MANAGER";

        public static BaseTransactionManager GetOrSetTransactionManager(Func<BaseTransactionManager> createTransactionMananger)
        {
            var transactionManager = GetCurrentThreadTransactionManager();

            if (transactionManager != null)
            {
                return transactionManager;
            }

            transactionManager = createTransactionMananger();

            if (TransactionSwitch.IsUseTransaction())
            {
                transactionManager.BeginTransaction();
            }

            SetCurrentThreadTransactionManager(transactionManager);

            return transactionManager;
        }

        public static BaseTransactionManager GetCurrentThreadTransactionManager()
        {
            return ThreadStaticStorage.GetData(TRANSACTION_MANAGER) as BaseTransactionManager;
        }

        public static void ClearCurrentThreadTransactionManager()
        {
            ThreadStaticStorage.FreeNamedDataSlot(TRANSACTION_MANAGER);
        }

        private static void SetCurrentThreadTransactionManager(BaseTransactionManager transactionManager)
        {
            ThreadStaticStorage.SetData(TRANSACTION_MANAGER, transactionManager);
        }
    }
}

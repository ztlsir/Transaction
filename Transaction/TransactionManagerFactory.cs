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

            if (BaseTransactionManager.IsUseTransaction())
            {
                transactionManager.BeginTransaction();
            }

            SetCurrentThreadTransactionManager(transactionManager);

            return transactionManager;
        }

        public static BaseTransactionManager GetCurrentThreadTransactionManager()
        {
            var connDataSlot = Thread.GetNamedDataSlot(TRANSACTION_MANAGER);
            if (connDataSlot == null)
            {
                return null;
            }

            return Thread.GetData(connDataSlot) as BaseTransactionManager;
        }

        private static void SetCurrentThreadTransactionManager(BaseTransactionManager transactionManager)
        {
            var connDataSlot = GetOrAllocateNamedDataSlot(TRANSACTION_MANAGER);
            Thread.SetData(connDataSlot, transactionManager);
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

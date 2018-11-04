using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transaction
{
    public class TransactionManager
    {
        private const string TRAN_FLAG_SLOT = "TRAN_FLAG_SLOT";

        public static void TurnOn()
        {
            SetTranFlag(true);
        }

        public static void TurnOff()
        {
            SetTranFlag(false);
        }

        private static void SetTranFlag(bool isTran)
        {
            var isTranDataSlot = DBConnectionManager.GetOrAllocateNamedDataSlot(TRAN_FLAG_SLOT);
            Thread.SetData(isTranDataSlot, isTran);
        }

        public static bool IsUseTransaction()
        {
            var isTranDataSlot = DBConnectionManager.GetOrAllocateNamedDataSlot(TRAN_FLAG_SLOT);
            var data = Thread.GetData(isTranDataSlot);

            if (data == null)
            {
                return false;
            }

            return (bool)data;
        }
    }
}

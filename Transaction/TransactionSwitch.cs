using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transaction
{
    public class TransactionSwitch
    {
        protected const string TRAN_FLAG_SLOT = "TRAN_FLAG_SLOT";

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
            ThreadStaticStorage.SetData(TRAN_FLAG_SLOT, isTran);
        }

        public static bool IsUseTransaction()
        {
            var data = ThreadStaticStorage.GetData(TRAN_FLAG_SLOT);

            if (data == null)
            {
                return false;
            }

            return (bool)data;
        }
    }
}

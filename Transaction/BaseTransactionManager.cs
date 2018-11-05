using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transaction
{
    public abstract class BaseTransactionManager
    {
        public abstract void BeginTransaction();

        public abstract void Commit();

        public abstract void Rollback();

        public abstract void Dispose();
    }
}

using LightInject.Interception;
using System;
using System.Linq;

namespace Transaction.LightInject
{
    public class TransactionInterceptor : BaseTransactionInterceptor, IInterceptor
    {
        public object Invoke(IInvocationInfo invocationInfo)
        {
            if (invocationInfo.Method.CustomAttributes.ToList().Exists(e => e.AttributeType == typeof(TransactionAttribut)))
            {
                object proceedResult = null;
                ExecuteByTransection(() =>
                {
                    proceedResult = invocationInfo.Proceed();
                });

                return proceedResult;
            }

            return invocationInfo.Proceed();
        }
    }
}

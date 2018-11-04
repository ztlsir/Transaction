using LightInject;
using LightInject.Interception;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Transaction.LightInject;
using Transaction.Test.LightInjectTest.TestClass;

namespace Transaction.Test.LightInjectTest
{
    [TestClass]
    public class LightInjectTest
    {
        [TestInitialize]
        public void SetUp() 
        {

        }

        [TestMethod]
        public void InterceptorTest()
        {
            var container = new ServiceContainer();
            container.Register<InterceptorTest>();
            container.Intercept(sr =>
            {
                return sr.ServiceType == typeof(InterceptorTest);
            }, sf => new TransactionInterceptor());

            var interceptorTest = container.GetInstance<InterceptorTest>();
            interceptorTest.AddTwoUser(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                new InterceptorTest.User
                {
                    CreateTime = DateTime.Now,
                    Date = "test_date",
                    Name = "test_name",
                    Number = "test_number"
                });
        }
    }
}

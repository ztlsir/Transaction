using LightInject;
using LightInject.Interception;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            using (var container = new ServiceContainer())
            {
                container.Register<InterceptorTest>();
                container.Intercept(sr =>
                {
                    return sr.ServiceType == typeof(InterceptorTest);
                }, sf => new TransactionInterceptor());

                var interceptorTest = container.GetInstance<InterceptorTest>();
                interceptorTest.AddTwoUser(
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    new User
                    {
                        CreateTime = DateTime.Now,
                        Date = "test_date",
                        Name = "test_name",
                        Number = "test_number"
                    });
            }
        }

        [TestMethod]
        public void TaskTest()
        {
            using (var container = new ServiceContainer())
            {
                container.Register<InterceptorTest>();
                container.Intercept(sr =>
                {
                    return sr.ServiceType == typeof(InterceptorTest);
                }, sf => new TransactionInterceptor());
                Task.Run(() =>
                {
                    var interceptorTest = container.GetInstance<InterceptorTest>();

                    interceptorTest.AddTwoUser(
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        new User
                        {
                            CreateTime = DateTime.Now,
                            Date = "test_date",
                            Name = "test_name",
                            Number = "test_number"
                        });
                }).Wait();
            }
        }

        [TestMethod]
        public void ThreadsTest()
        {
            using (var container = new ServiceContainer())
            {
                container.Register<InterceptorTest>();
                container.Register<InterceptorTest2>();
                container.Intercept(sr =>
                {
                    return sr.ServiceType == typeof(InterceptorTest);
                }, sf => new TransactionInterceptor());
                container.Intercept(sr =>
                {
                    return sr.ServiceType == typeof(InterceptorTest2);
                }, sf => new TransactionInterceptor());

                List<Action> actions = new List<Action>();
                actions.Add(() =>
                {
                    var interceptorTest = container.GetInstance<InterceptorTest>();
                    interceptorTest.AddTwoUser(
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        new User
                        {
                            CreateTime = DateTime.Now,
                            Date = "test_date",
                            Name = "test_name",
                            Number = "test_number"
                        });
                });

                actions.Add(() =>
                {
                    var interceptorTest = container.GetInstance<InterceptorTest2>();
                    interceptorTest.AddTwoUserToTest2(
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        new User
                        {
                            CreateTime = DateTime.Now,
                            Date = "test_date",
                            Name = "test_name",
                            Number = "test_number"
                        });
                });

                Parallel.ForEach(
                    actions,
                    new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = actions.Count
                    },
                    action =>
                    {
                        action();
                    });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.LightInject;
using Transaction.Dapper;

namespace Transaction.Test.LightInjectTest.TestClass
{
    public class InterceptorTest
    {
        [TransactionAttribut]
        public virtual void AddTwoUser(string firstId, string secondId, User user)
        {
            user.Id = firstId;
            AddUserInfo(user);

            user.Id = secondId;
            AddUserInfo(user);
        }

        public int AddUserInfo(User user)
        {
            var conn = new DapperConnection("Server=.;database=test;uid=sa;pwd=123456");
            return conn.Execute(
                "insert into [Users](Id,Name,Number,Date,CreateTime) values(@Id,@Name,@Number,@Date,@CreateTime)",
                user);
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public string Date { get; set; }
            public DateTime CreateTime { get; set; }
        }
    }
}

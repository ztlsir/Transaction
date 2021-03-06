﻿using System;
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
        [TransactionAttribut]
        public virtual void AddTwoUserToTest2(string firstId, string secondId, User user)
        {
            user.Id = firstId;
            AddUserInfoToTest2(user);

            user.Id = secondId;
            AddUserInfoToTest2(user);
        }

        private int AddUserInfo(User user)
        {
            var conn = new DapperConnection("Server=.;database=test;uid=sa;pwd=123456");
            return conn.Execute(
                "insert into [Users](Id,Name,Number,Date,CreateTime) values(@Id,@Name,@Number,@Date,@CreateTime)",
                user);
        }

        private int AddUserInfoToTest2(User user)
        {
            var conn = new DapperConnection("Server=.;database=test2;uid=sa;pwd=123456");
            return conn.Execute(
                "insert into [Users2](Id,Name,Number,Date,CreateTime) values(@Id,@Name,@Number,@Date,@CreateTime)",
                user);
        }
    }
}

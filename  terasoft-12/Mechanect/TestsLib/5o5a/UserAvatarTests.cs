using System;
using NUnit.Framework;
using Mechanect;
using Mechanect.Exp3;
using Mechanect.Common;

namespace Tests
{
    [TestFixture]
    public class UserAvatarTests
    {
        User user;
        User[] users;
         [SetUp]
        public void Init()
        {
             user = new User(); ;
            users= new User[4];
            users[0] = user;
        }

        [Test]
        public void TestGetUserIndex()
        {
            Assert.AreEqual(UserAvatar.getUserindex(user,users), user);
        }
    }
}

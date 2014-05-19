using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ResourcesFirstTranslations.Common;

namespace ResourcesFirstTranslations.Tests
{
    [TestFixture]
    public class PasswordTests
    {
        [Test]
        public void _GenerateAdminPassword()
        {
            var password = Password.GeneratePassword();
            var hashedAndSalt = Password.HashPassword(password);

            Debug.WriteLine("password: {0}, hashed password: {1}, salt: {2}", 
                password, hashedAndSalt.Item1, hashedAndSalt.Item2);

            Assert.Ignore();
        }
    }
}

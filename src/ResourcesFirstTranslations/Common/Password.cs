using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCrypto;

namespace ResourcesFirstTranslations.Common
{
    public static class Password
    {
        // https://github.com/shawnmclean/SimpleCrypto.net
        public static bool IsPasswordValid(string password, string storedPasswordHash, string salt)
        {
            ICryptoService cryptoService = new PBKDF2();
            string hashedPassword2 = cryptoService.Compute(password, salt);

            return cryptoService.Compare(storedPasswordHash, hashedPassword2);
        }

        public static Tuple<string, string> HashPassword(string password)
        {
            ICryptoService cryptoService = new PBKDF2();

            string salt = cryptoService.GenerateSalt();
            string hashedPassword = cryptoService.Compute(password, salt);

            return new Tuple<string, string>(hashedPassword, salt);
        }

        public static string GeneratePassword()
        {
            return RandomPassword.Generate(8, PasswordGroup.Uppercase, PasswordGroup.Lowercase);
        }
    }
}

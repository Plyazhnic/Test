using System;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using EXP.Entity.Enumerations;

namespace EXP.Core.Util
{
    public static class CryptoUtils
    {
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);

            //TODO: Make this option configurable
            //string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");

            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, EncryptionTypeLookup.MD5.ToString());
            return hashedPwd;
        }

        public static string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);

            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        public static string DecodeFromBase64(string toDecode)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(toDecode);

            string returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }
    }
}

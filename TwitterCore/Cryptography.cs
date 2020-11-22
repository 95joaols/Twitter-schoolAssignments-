using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DevOne.Security.Cryptography.BCrypt;

namespace TwitterCore
{
    internal static class Cryptography
    {
        public static string Encrypt(string encryptString, string salt)
        {
            string EncryptionKey = salt;
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using(Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
                {
                    0x49,
                    0x76,
                    0x61,
                    0x6e,
                    0x20,
                    0x4d,
                    0x65,
                    0x64,
                    0x76,
                    0x65,
                    0x64,
                    0x65,
                    0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using(MemoryStream ms = new MemoryStream())
                {
                    using(CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Hash(string text, string salt)
        {
            return BCryptHelper.HashPassword(text, salt);
        }

        public static string CreatSalt()
        {
            return BCryptHelper.GenerateSalt();
        }
    }
}
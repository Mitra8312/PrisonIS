using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PrisonAdministration
{
    internal class RegistryTrash
    {

        public static void SaveUserCredentials()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\SolovkiPrison");
            key.SetValue("Login", EncryptString(App.login));
            key.SetValue("Password", EncryptString(App.password));
        }

        public static void GetUserCredentials()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\SolovkiPrison");
            App.login = DecryptString(key.GetValue("Login", "").ToString());
            App.password = DecryptString(key.GetValue("Password", "").ToString());
        }

        private static readonly byte[] Salt = new byte[] { 0x26, 0xdc, 0xab, 0x19, 0xf1, 0x25, 0x6e, 0x7d, 0x45, 0xa3, 0xb2, 0xfc, 0x15, 0x79, 0x86, 0x34 };

        public static string EncryptString(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes("GOSHAKRUTOI", Salt);
                aes.Key = keyDerivation.GetBytes(32);
                aes.IV = keyDerivation.GetBytes(16);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(BitConverter.GetBytes(plainBytes.Length), 0, sizeof(int));
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    byte[] cipherBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherBytes);
                }
            }
        }

        public static string DecryptString(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes("GOSHAKRUTOI", Salt);
                aes.Key = keyDerivation.GetBytes(32);
                aes.IV = keyDerivation.GetBytes(16);
                using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
                {
                    byte[] lengthBytes = new byte[sizeof(int)];
                    memoryStream.Read(lengthBytes, 0, sizeof(int));
                    int plainLength = BitConverter.ToInt32(lengthBytes, 0);
                    byte[] plainBytes = new byte[plainLength];
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                    }
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }
    }
}

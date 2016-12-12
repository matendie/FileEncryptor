using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

namespace FileCryptography
{
    public static class Encryption
    {
         
        //private static SHA512Managed getKey(string)
        //{
        //    byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        //    derive pwdGen = new SHA512Managed(sKey, salt, 1000);
        //    return pwdGen;
        //}

        private static Rfc2898DeriveBytes getKey(string sKey)
        { 
            byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(sKey, salt, 1000); 
            return pwdGen;
        }

        public static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {

            Rfc2898DeriveBytes pwdGen = getKey(sKey);
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
            rc2.Key = pwdGen.GetBytes(16);
            rc2.IV = pwdGen.GetBytes(8); 
             
            ICryptoTransform desencrypt = rc2.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }

        public static bool DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            try
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();

                Rfc2898DeriveBytes pwdGen = getKey(sKey);
                rc2.Key = pwdGen.GetBytes(16);
                rc2.IV = pwdGen.GetBytes(8);

                using (FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read))
                {

                    ICryptoTransform desdecrypt = rc2.CreateDecryptor();

                    using (CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
                    {

                        using (StreamWriter fsDecrypted = new StreamWriter(sOutputFilename))
                        {
                            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                            fsDecrypted.Flush();
                            fsDecrypted.Close();
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect Password");
                return false;
            }
            
        }
    }
}

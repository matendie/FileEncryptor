﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace FileCryptography
{
    public class Encryption
    {
        public static bool ValidateKeyLength(string key)
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            bool result = desCrypto.ValidKeySize(key.Length);
            return result;
        }

        public static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }

        public static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            using (FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read))
            {
                //Create a file stream to read the encrypted file back. 
                //Create a DES decryptor from the DES instance.
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //Create crypto stream set to read and do a 
                //DES decryption transform on incoming bytes.
                using (CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
                {
                    //Print the contents of the decrypted file.
                    using (StreamWriter fsDecrypted = new StreamWriter(sOutputFilename))
                    {
                        fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                        fsDecrypted.Flush();
                        fsDecrypted.Close();
                    }
                }
            }
        }
    }
}

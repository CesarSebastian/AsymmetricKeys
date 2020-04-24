using System;
using System.IO;
using System.Text;

using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AsymmetricKeys
{
    class Program
    {
        public enum Mode
        {
            ENCRYPT, 
            DECRYPT
        }

        private static string key = "S2V5SGVhZGVycw=";
        private static string iv = "SXZIZWFkZXJz";
        
        static void Main(string[] args)
        {
            string message = "https://10.81.11.203:8095/WSBienestarAzteca/BienestarAzteca/operacionesGobierno/validaRetiro";
            
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;

                //aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
                //aes.IV = System.Text.UTF8Encoding.ASCII.GetBytes(iv);

                aes.GenerateIV();
                aes.GenerateKey();

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                Console.WriteLine(aes.Key.Length);

                byte[] encrypted = AESCrypto(Mode.ENCRYPT, aes, Encoding.UTF8.GetBytes(message));
                Console.WriteLine("Encrypted text: "+BitConverter.ToString(encrypted).Replace("-",""));

                byte[] decrypted = AESCrypto(Mode.DECRYPT, aes, encrypted);
                Console.WriteLine("Decryp text: " + Encoding.UTF8.GetString(decrypted));
            }
        }

        private static byte[] AESCrypto(Mode mode, AesCryptoServiceProvider aes, byte[] v)
        {
            using (var memStream = new MemoryStream())
            {
                CryptoStream cryptoSteam = null;

                if(mode == Mode.ENCRYPT)
                
                    cryptoSteam = new CryptoStream(memStream, aes.CreateEncryptor(),CryptoStreamMode.Write);
                
                else if (mode == Mode.DECRYPT)
                
                    cryptoSteam = new CryptoStream(memStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                
                if (cryptoSteam == null)
                
                    return null;
                

                cryptoSteam.Write(v, 0, v.Length);
                cryptoSteam.FlushFinalBlock();
                return memStream.ToArray();
            }
        }
    }
}

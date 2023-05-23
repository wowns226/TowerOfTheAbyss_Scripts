// Copyright (C) <2023>  
//     Authors : Shin YongUk <dyddyddnr@naver.com>
//     Lim jaejun <wowns226@naver.com>
//  
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//  
//     You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ProjectL
{
    public static class AesEncryptor
    {
        private const int KEY_SIZE = 256;
        private const int BLOCK_SIZE = 128;
        private const string KEY = "01234567890123456789012345678901";
        private const string IV = "1234567890123456";

        public static byte[] Encrypt(string plainText)
        {
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = KEY_SIZE;
                aes.BlockSize = BLOCK_SIZE; 
                aes.Key = Encoding.UTF8.GetBytes(KEY);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static string Decrypt(byte[] cipherText)
        {
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = KEY_SIZE;
                aes.BlockSize = BLOCK_SIZE; 
                aes.Key = Encoding.UTF8.GetBytes(KEY);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(cipherText))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            plaintext = sr.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}

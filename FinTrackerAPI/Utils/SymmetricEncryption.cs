using System.Security.Cryptography;

namespace FinTrackerAPI.Utils
{
    public class SymmetricEncryption
    {
        private static string key = "tpqjtybm9H+Q6JykzacEz7rxJxHzh6DQG6ed/lD7ynU=";
        private static string iv = "FtxFo4tSRPeYNoqDp4v0hQ==";

        internal static byte[] Encrypt(string message)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(message);
            }
            return ms.ToArray();
        }

        internal static string Decrypt(byte[] cipherText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}

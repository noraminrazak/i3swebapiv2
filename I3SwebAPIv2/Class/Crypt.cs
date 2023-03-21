using System;
using System.Security.Cryptography;
using System.Text;

namespace I3SwebAPIv2.Class
{
    public class Crypt
    {
        public enum RSAKeySize
        {
            Key512 = 512,
            Key1024 = 1024,
            Key2048 = 2048,
            Key4096 = 4096
        }

        public class RSAKeysTypes
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public class RSACryptographyKeyGenerator
        {
            public RSAKeysTypes GenerateKeys(RSAKeySize rsaKeySize)
            {
                int keySize = (int)rsaKeySize;
                if (keySize % 2 != 0 || keySize < 512)
                    throw new Exception("Key should be multiple of two and greater than 512.");

                var rsaKeysTypes = new RSAKeysTypes();

                using (var provider = new RSACryptoServiceProvider(keySize))
                {
                    var publicKey = provider.ToXmlString(false);
                    var privateKey = provider.ToXmlString(true);

                    var publicKeyWithSize = IncludeKeyInEncryptionString(publicKey, keySize);
                    var privateKeyWithSize = IncludeKeyInEncryptionString(privateKey, keySize);

                    rsaKeysTypes.PublicKey = publicKeyWithSize;
                    rsaKeysTypes.PrivateKey = privateKeyWithSize;
                }

                return rsaKeysTypes;
            }

            internal object GenerateKeys(object key512)
            {
                throw new NotImplementedException();
            }

            private string IncludeKeyInEncryptionString(string publicKey, int keySize)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(keySize.ToString() + "!" + publicKey));
            }
        }

        public string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
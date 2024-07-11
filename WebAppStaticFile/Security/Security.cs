using System.Security.Cryptography;
using System.Text;

namespace WebAppStaticFile.Security
{
    public static class Security
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string EncryptValue(string key, string value)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.ASCII.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(value);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            var result = Convert.ToBase64String(array);
            return result;
        }

        public static string DecryptValue(this string encryptValue)
        {
            encryptValue = encryptValue.ToString().Replace(' ', '+');
            var key = "6A586E327235753778214125442A472F";
            var resultEndcode = string.Empty;
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(encryptValue.ToString());
                using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
                {
                    aes.Key = Encoding.ASCII.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                resultEndcode = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                resultEndcode = "";
            }
            return resultEndcode;
        }

        public static bool CheckEncryptValueDate(string dateValue,int hour)
        {
            if (!string.IsNullOrEmpty(dateValue))
            {
                try
                {
                    long dateTick = long.Parse(dateValue);
                    long dateNow = DateTime.UtcNow.Ticks;
                    if ((dateNow - hour * 36000000000) <= dateTick)
                        return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}

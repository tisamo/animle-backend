using Animle.Classes;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptionHelper
{
    private readonly byte[] key;
    private readonly byte[] iv; 

    private readonly ConfigSettings _appSettings;

    public EncryptionHelper(IOptions<ConfigSettings> options)
    {
        _appSettings = options.Value;
        key = Convert.FromBase64String(_appSettings.Aes); 
        iv = Encoding.UTF8.GetBytes(_appSettings.AesIV);
    }

    public string Encrypt(dynamic obj)
    {
        var plainText = JsonConvert.SerializeObject(obj);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                byte[] encrypted = msEncrypt.ToArray();
                return Convert.ToBase64String(encrypted);
            }
        }
    }
}

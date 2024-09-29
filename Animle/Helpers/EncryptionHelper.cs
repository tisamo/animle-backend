using System.Security.Cryptography;
using System.Text;
using Animle.Classes;
using Microsoft.Extensions.Options;

namespace Animle.Helpers;

public class EncryptionHelper
{
    private readonly ConfigSettings _appSettings;

    public EncryptionHelper(IOptions<ConfigSettings> options)
    {
        _appSettings = options.Value;
    }

    public byte[] Encrypt(byte[] plainBytes)
    {
        var secret = _appSettings.HashingSercret;
        var key = Encoding.UTF8.GetBytes(secret);
        byte[] encryptedBytes;

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using (var encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
        }

        return encryptedBytes;
    }
}
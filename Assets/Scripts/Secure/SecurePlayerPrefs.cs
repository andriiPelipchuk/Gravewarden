using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

public static class SecurePlayerPrefs
{
    private static readonly byte[] key = Encoding.UTF8.GetBytes("my_secret_key_123456789012345678");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("my_secret_iv_123");

    public static void SetEncryptedString(string keyName, string value)
    {
        string encrypted = Encrypt(value);
        PlayerPrefs.SetString(keyName, encrypted);
        PlayerPrefs.Save();
    }

    public static string GetDecryptedString(string keyName)
    {
        if (!PlayerPrefs.HasKey(keyName))
            return null;

        string encrypted = PlayerPrefs.GetString(keyName);
        return Decrypt(encrypted);
    }

    private static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes;

            using (var encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    private static string Decrypt(string encryptedText)
    {
        try
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes;

                using (var decryptor = aes.CreateDecryptor())
                {
                    decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                }

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        catch
        {
            Debug.LogWarning("Помилка дешифрування — можливо, файл зіпсований або змінений.");
            return null;
        }
    }
}

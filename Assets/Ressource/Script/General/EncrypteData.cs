using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;

public class EncrypteData : MonoBehaviour
{
    private static readonly string encryptionKey = "ImWkBl9w4yM8Zw0dac9Xzw7LtduwyI8g"; // Clé de chiffrement statique

    protected string EncryptData(string data)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(data);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey); // Utilisez la clé de chiffrement stockée

            // Générer un vecteur d'initialisation (IV)
            aes.GenerateIV();

            // Chiffrer les données
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            // Concaténer l'IV aux données chiffrées
            byte[] combinedBytes = new byte[aes.IV.Length + encryptedBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, combinedBytes, 0, aes.IV.Length);
            Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, aes.IV.Length, encryptedBytes.Length);

            return Convert.ToBase64String(combinedBytes);
        }
    }

    protected string DecryptData(string encryptedData)
    {
        byte[] combinedBytes = Convert.FromBase64String(encryptedData);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey); // Utilisez la clé de chiffrement stockée

            // Extraire l'IV des données chiffrées
            byte[] iv = new byte[aes.IV.Length];
            Buffer.BlockCopy(combinedBytes, 0, iv, 0, iv.Length);

            // Extraire les données chiffrées sans l'IV
            byte[] encryptedBytes = new byte[combinedBytes.Length - aes.IV.Length];
            Buffer.BlockCopy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            // Déchiffrer les données
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}

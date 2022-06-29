using System;
using System.Security.Cryptography;

namespace RockslopeAPI.Helpers;

public class Security
{
    const int iterations = 10000;
    const int saltLength = 16;
    const int hashLength = 20;
    
    public static string Encrypt(string toEncrypt)
    {
        byte[] salt = new byte[saltLength];
#pragma warning disable SYSLIB0023
        new RNGCryptoServiceProvider().GetBytes(salt);
#pragma warning restore SYSLIB0023

        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(toEncrypt, salt, iterations);

        byte[] hash = pbkdf2.GetBytes(hashLength);
        byte[] hashBytes = new byte[saltLength + hashLength];

        Array.Copy(salt, 0, hashBytes, 0, saltLength);
        Array.Copy(hash, 0, hashBytes, saltLength, hashLength);

        return Convert.ToBase64String(hashBytes);
    }
    
    static bool Compare(string enteredPasswordString, string databasePasswordString)
    {
        byte[] enteredPassword = Convert.FromBase64String(enteredPasswordString);
        byte[] databasePassword = Convert.FromBase64String(databasePasswordString);

        if (enteredPassword.Length != databasePassword.Length)
        {
            return false;
        }

        for (int i = 0; i < enteredPassword.Length; i++)
        {
            if (enteredPassword[i] != databasePassword[i])
            {
                return false;
            }
        }

        return true;
    }
    
    static bool EncryptAndCompare(string password, string original)
    {
        string savedPassword = original;
        byte[] hashBytes = Convert.FromBase64String(savedPassword);

        byte[] salt = new byte[saltLength];
        Array.Copy(hashBytes, 0, salt, 0, saltLength);

        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] hash = pbkdf2.GetBytes(hashLength);

        bool b = true;
        for (int i = 0; i < hashLength; i++)
        {
            if (hashBytes[i + saltLength] != hash[i])
            {
                b = false;
            }
        }

        return b;
    }
}
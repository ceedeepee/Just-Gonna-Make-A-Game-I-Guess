using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mPlayer
{
    public class ExtensionMethods : MonoBehaviour
    {
        public static ExtensionMethods Instance;

        private string key = "b59226a8e0039ef3ff78f23c8a4ede65";
        private string iv = "2f5c63199a6d45dc";
        private void Awake()
        {
            Instance = this;
        }

        public static Vector3 Round(Vector3 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new Vector3(
                Mathf.Round(vector3.x * multiplier) / multiplier,
                Mathf.Round(vector3.y * multiplier) / multiplier,
                Mathf.Round(vector3.z * multiplier) / multiplier);
        }
        public string AESEncryption(string inputData)
        {
            AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
            AEScryptoProvider.BlockSize = 128;
            AEScryptoProvider.KeySize = 256;
            AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
            AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            AEScryptoProvider.Mode = CipherMode.CBC;
            AEScryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(inputData);
            ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

            byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
            return ByteArrayToString(result);
        }
        public string AESDecryption(string inputData)
        {
            AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
            AEScryptoProvider.BlockSize = 128;
            AEScryptoProvider.KeySize = 256;
            AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
            AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            AEScryptoProvider.Mode = CipherMode.CBC;
            AEScryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] txtByteData = StringToByteArray(inputData);
            ICryptoTransform trnsfrm = AEScryptoProvider.CreateDecryptor();

            byte[] byteResult = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
            return ASCIIEncoding.ASCII.GetString(byteResult);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

    }
}
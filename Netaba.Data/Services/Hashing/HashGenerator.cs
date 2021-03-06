﻿using System.Security.Cryptography;
using System.Text;

namespace Netaba.Data.Services.Hashing
{
    public class HashGenerator
    {
        public static byte[] GetHash(string ip, string password)
        {
            return SHA512.HashData(Encoding.UTF8.GetBytes(ip + password));
        }

        public static byte[] GetHash(string password)
        {
            return SHA512.HashData(Encoding.UTF8.GetBytes(password));
        }
    }
}

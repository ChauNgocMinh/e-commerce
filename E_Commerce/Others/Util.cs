using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Others
{
    public class Util
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Util(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public string GetIpAddress()
        {
            string ipAddress;
            try
            {
                HttpContext context = _httpContextAccessor.HttpContext;

                ipAddress = context.Request.Headers["X-Forwarded-For"];
                if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
                    ipAddress = context.Connection.RemoteIpAddress?.ToString();

            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
    }
}
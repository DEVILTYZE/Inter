using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Inter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inter.Helpers
{
    public static class AccountHelper
    {
        public const string ConstString = "n071n73r";

        public static string GetHashedPassword(string password, string email)
        {
            password = GetHash(password);
            var salt = GetSalt(email);
            var indexOfPasswordCenter = password.Length / 2;
            var indexOfSaltCenter = salt.Length / 2;

            return password[..indexOfPasswordCenter] + salt[indexOfSaltCenter..] + password[indexOfPasswordCenter..]
                   + salt[..indexOfSaltCenter] + ConstString;
        }

        public static int GetAccessIndex(ClaimsPrincipal user, string accessRoleName)
        {
            var role = user.Claims.FirstOrDefault(claim => string.CompareOrdinal(claim.Type, 
                ClaimsIdentity.DefaultRoleClaimType) == 0) ?? new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleName.Anon);

            return GetAccessIndex(role.Value, accessRoleName);
        }

        public static int GetAccessIndex(string currentRoleName, string accessRoleName)
        {
            int indexRole = RoleName.GetRoleIndex(currentRoleName), indexAccessRole = RoleName.GetRoleIndex(accessRoleName);

            if (indexRole < indexAccessRole)
                return 1;
            
            if (indexRole == indexAccessRole)
                return 0;

            return -1;
        }

        public static string GetIpAddress(HttpContext context) 
            => context.Connection.RemoteIpAddress is null ? "0.0.0.0" : context.Connection.RemoteIpAddress.ToString();

        public static SelectList GetSelectList(IEnumerable<Role> items)
        {
            var selectList = new SelectList(items.Where(item => string.CompareOrdinal(
                item.Name, RoleName.Banned) != 0), "Name", "Name");
            selectList.First(item => item.Text == RoleName.Anon).Selected = true;

            return selectList;
        }
        
        private static string GetHash(string password)
        {
            using var hasher = new SHA1Managed();
            var hash = Encoding.Default.GetString(hasher.ComputeHash(Encoding.Default.GetBytes(password)));

            return hash;
        }

        private static string GetSalt(string email)
        {
            var index = email.IndexOf('@');
            var emailName = email[..index].Replace(".", "");
            var emailEnd = email[(index + 1)..].Replace(".", "");

            return emailEnd + emailName;
        }
        
    }
}
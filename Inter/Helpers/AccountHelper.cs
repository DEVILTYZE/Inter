using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
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
            var salt = GetSalt(email);
            var indexOfPasswordCenter = password.Length / 2;
            var indexOfSaltCenter = salt.Length / 2;
            password = password[..indexOfPasswordCenter] + salt[indexOfSaltCenter..] + password[indexOfPasswordCenter..]
                       + salt[..indexOfSaltCenter] + ConstString;
            
            return GetHash(password);
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

        public static bool IsMainController(string path)
        {
            var controller = GetController(path);

            return string.CompareOrdinal(controller, "Board") == 0 ||
                   string.CompareOrdinal(controller, "Thread") == 0 ||
                   string.CompareOrdinal(controller, "Post") == 0;
        }

        public static string GetIpAddress(HttpContext context) 
            => context.Connection.RemoteIpAddress is null ? "0.0.0.0" : context.Connection.RemoteIpAddress.ToString();

        public static SelectList GetSelectList(IEnumerable<Role> items)
        {
            var selectList = new SelectList(items.Where(item => string.CompareOrdinal(
                item.Name, RoleName.Banned) != 0), "Name", "Name");

            if (selectList.Any(item => string.CompareOrdinal(item.Text, RoleName.Anon) == 0))
                selectList.First(item => string.CompareOrdinal(item.Text, RoleName.Anon) == 0).Selected = true;
            else
                selectList.First(item => string.CompareOrdinal(item.Text, RoleName.User) == 0).Selected = true;

            return selectList;
        }

        public static SelectList[] GetWriteAndReadSelectLists(IEnumerable<Role> items)
        {
            var itemsArray = items.ToArray();
            var lists = new SelectList[2];
            lists[0] = GetSelectList(itemsArray);
            lists[1] = GetSelectList(itemsArray.Where(item => string.CompareOrdinal(item.Name, RoleName.Anon) != 0));

            return lists;
        }

        private static string GetHash(string passwordAndSalt)
        {
            var hasher = new Sha3Digest(512);
            var input = Encoding.ASCII.GetBytes(passwordAndSalt);
            
            hasher.BlockUpdate(input, 0, input.Length);

            var result = new byte[64];

            hasher.DoFinal(result, 0);
            var hash = BitConverter.ToString(result);
            
            return hash.Replace("-", "").ToLowerInvariant();

            // using var hasher = new SHA256Managed();
            // var hash = Encoding.Default.GetString(hasher.ComputeHash(Encoding.Default.GetBytes(passwordAndSalt)));
            //
            // return hash;
        }

        private static string GetSalt(string email)
        {
            var index = email.IndexOf('@');
            var emailName = email[..index].Replace(".", "");
            var emailEnd = email[(index + 1)..].Replace(".", "");

            return emailEnd + emailName;
        }

        private static string GetController(string path)
        {
            if (path.Length == 1 && string.CompareOrdinal(path, "/") == 0)
                return "Board";

            var splitPath = path.Split('/');
            path = splitPath[1];
            var indexOfBreakPoint = path.IndexOf('?');

            return indexOfBreakPoint != -1 ? path[..indexOfBreakPoint] : path;
        }
    }
}
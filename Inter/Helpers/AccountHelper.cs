using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Digests;
using Inter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inter.Helpers
{
    /// <summary>
    /// Статический класс, помогающий решать проблемы, связанные с аккаунтом.
    /// </summary>
    public static class AccountHelper
    {
        public const string ConstString = "n071n73r";

        /// <summary>
        /// Метод преобразующий пароль и email в хэш.
        /// </summary>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Хэш-строка.</returns>
        public static string GetHashedPassword(string password, string email)
        {
            var salt = GetSalt(email);
            var indexOfPasswordCenter = password.Length / 2;
            var indexOfSaltCenter = salt.Length / 2;
            password = password[..indexOfPasswordCenter] + salt[indexOfSaltCenter..] + password[indexOfPasswordCenter..]
                       + salt[..indexOfSaltCenter] + ConstString;
            
            return GetHash(password);
        }

        /// <summary>
        /// Метод, позволяющий получить индекс доступа.
        /// 1 — роль пользователя выше роли доступа;
        /// 0 — роль пользователя равна роли доступа;
        /// -1 — роль пользователя ниже роли доступа;
        /// </summary>
        /// <param name="user">Текущий пользователь.</param>
        /// <param name="accessRoleName">Имя роли доступа.</param>
        /// <returns>Целое число от -1 до 1.</returns>
        public static int GetAccessIndex(ClaimsPrincipal user, string accessRoleName)
        {
            var role = user.Claims.FirstOrDefault(claim => string.CompareOrdinal(claim.Type, 
                ClaimsIdentity.DefaultRoleClaimType) == 0) ?? new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleName.Anon);

            return GetAccessIndex(role.Value, accessRoleName);
        }

        /// <summary>
        /// Метод, позволяющий получить индекс доступа.
        /// 1 — роль пользователя выше роли доступа;
        /// 0 — роль пользователя равна роли доступа;
        /// -1 — роль пользователя ниже роли доступа;
        /// </summary>
        /// <param name="currentRoleName">Имя роли пользователя.</param>
        /// <param name="accessRoleName">Имя роли доступа.</param>
        /// <returns>Целое число от -1 до 1.</returns>
        public static int GetAccessIndex(string currentRoleName, string accessRoleName)
        {
            int indexRole = RoleName.GetRoleIndex(currentRoleName), indexAccessRole = RoleName.GetRoleIndex(accessRoleName);

            if (indexRole < indexAccessRole)
                return 1;
            
            if (indexRole == indexAccessRole)
                return 0;

            return -1;
        }

        /// <summary>
        /// Метод, позволяющий узнать, является ли текущий контроллер основным.
        /// Основным контроллером считаются контроллеры Board, Thread, Post.
        /// </summary>
        /// <param name="path">Текущий url-путь.</param>
        /// <returns>Булевое значение True или False.</returns>
        public static bool IsMainController(string path)
        {
            var controller = GetController(path);

            return string.CompareOrdinal(controller, "Board") == 0 ||
                   string.CompareOrdinal(controller, "Thread") == 0 ||
                   string.CompareOrdinal(controller, "Post") == 0;
        }

        /// <summary>
        /// Метод позволяющий узнать ip-адрес текущего пользователя.
        /// </summary>
        /// <param name="context">Текущий HttpContext.</param>
        /// <returns>Строку формата "x.x.x.x", где x — целое число от 0 до 255.</returns>
        public static string GetIpAddress(HttpContext context) 
            => context.Connection.RemoteIpAddress is null ? "0.0.0.0" : context.Connection.RemoteIpAddress.ToString();

        /// <summary>
        /// Метод позволяющий получить объект класса SelectList с ролями.
        /// Исключается роль Banned.
        /// </summary>
        /// <param name="items">Список с ролями.</param>
        /// <returns>Объект класса SelectList.</returns>
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

        /// <summary>
        /// Метод позволяющий получить два объекта класса SelectList.
        /// Один используется для ролей, которые могут читать, второй используется для ролей, которые могут писать.
        /// </summary>
        /// <param name="items">Список с ролями.</param>
        /// <returns>Массив с объектами класса SelectList.</returns>
        public static SelectList[] GetWriteAndReadSelectLists(IEnumerable<Role> items)
        {
            var itemsArray = items.ToArray();
            var lists = new SelectList[2];
            lists[0] = GetSelectList(itemsArray);
            lists[1] = GetSelectList(itemsArray.Where(item => string.CompareOrdinal(item.Name, RoleName.Anon) != 0));

            return lists;
        }

        /// <summary>
        /// Метод позволяющий получить объект класса User с информацией о текущем пользователе.
        /// </summary>
        /// <param name="context">Текущий HttpContext.</param>
        /// <param name="db">Текущая база данных.</param>
        /// <returns>Объект класса User.</returns>
        /// <exception cref="Exception">Если текущего пользователя не существует ещё в контексте.</exception>
        public static async Task<User> GetCurrentUserAsync(HttpContext context, InterService db)
        {
            var builder = new FilterDefinitionBuilder<User>();

            if (context.User.Identity is null)
                throw new Exception("Class: AccountController; Method: GetCurrentUser.");
            
            var userName = context.User.Identity.Name;
            
            return await db.Users.Find(builder.Regex("Name", new BsonRegularExpression(userName))).FirstAsync();
        }

        /// <summary>
        /// Метод позволяющий получить объект класса User с информацией о текущем пользователе или null.
        /// </summary>
        /// <param name="context">Текущий HttpContext.</param>
        /// <param name="db">Текущая база данных.</param>
        /// <returns>Объект класса User или null.</returns>
        public static async Task<User> GetCurrentUserOrDefaultAsync(HttpContext context, InterService db)
        {
            try
            {
                return await GetCurrentUserAsync(context, db);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsAuthorized(HttpContext context) 
            => context.User.Identity is not null && context.User.Identity.IsAuthenticated;

        /// <summary>
        /// Метод, позволяющий получить хэш с помощью SHA3.
        /// </summary>
        /// <param name="passwordAndSalt">Строка с паролем и солью.</param>
        /// <returns>Хэш-строку.</returns>
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

        /// <summary>
        /// Метод позволяющий получить соль, используемую для пароля.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Строку.</returns>
        private static string GetSalt(string email)
        {
            var index = email.IndexOf('@');
            var emailName = email[..index].Replace(".", "");
            var emailEnd = email[(index + 1)..].Replace(".", "");

            return emailEnd + emailName;
        }

        
        /// <summary>
        /// Метод, позволяющий узнать используемый в данный момент контроллер.
        /// </summary>
        /// <param name="path">Текущий url-путь.</param>
        /// <returns>Название контроллера.</returns>
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
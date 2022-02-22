using System;

namespace Inter.Helpers
{
    public static class ConstHelper
    {
        public const string ConfigName = "config.json";
        
        public const string DefaultImageUrl = "_system/images/no_img.png";
        public const string DefaultSmallImageUrl = "_system/images/no_img_sl.png";
        public const string DefaultNormalImageUrl = "_system/images/no_img_nm.png";
        public const string DefaultLargeImageUrl = "_system/images/no_img_lg.png";
        public const string DefaultFilePicUrl = "_system/images/file_pic.png";
        public const string TempFolderUrl = "imgbrd/tmp";
        public const string LoginPath = "/Account/Login";

        public const string FilesFolderName = "files";
        public const string CompressedImageFolderName = "comp_img";
        public const string RandomThreadName = "Безымянный тред";
        public const string MainPageName = "Главная страница";

        public const string DateFormatSecs = "dd MMMM yyyy | HH:mm:ss";
        public const string DateFormat = "HH:mm | dd MMMM yyyy";
        public const int MaxNameLength = 30;
        public const int MaxFilesCount = 8;
        public const int MaxFileNameLength = 64;
        public const int CountAuditDocumentsPerPage = 500;
        
        // config
        
        public const long MaxFileSize = 10485760;
        public const int MaxTextLength = 35000;
        public const string LightThemeName = "light";
        public const string DarkThemeName = "dark";
        public const int MaxLoadThreadCount = 15;
        public const int MaxLoadPostCount = 25;
        public const int MaxLoadPreviewPostCount = 5;
    }

    public static class ConstError
    {
        public const string UserNameExist = "Пользователь с таким именем уже существует";
        public const string UserEmailExist = "Пользователь с таким email'ом уже существует";
        public const string WrongEmailOrPassword = "Неправильный email или пароль";
        public const string WrongCurrentPassword = "Некорректный текущий пароль";
        public const string YouAreBanned = "Вы забанены";
        
        public const string FileError404 = "FILE_ERROR_404";
        public const string PathError404 = "PATH_ERORR_404";
        public const string UserError404 = "USER_ERORR_404";
        public const string Success = "SUCCESS";
        public const string Failure = "FAILURE";
    }

    public static class RoleName
    {
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string User = "User";
        public const string Anon = "Anon";
        public const string Banned = "Banned";

        private static readonly string[] RoleList = { Admin, Moderator, User, Anon, Banned };

        public static int GetRoleIndex(string role) => Array.IndexOf(RoleList, role);
    }
}
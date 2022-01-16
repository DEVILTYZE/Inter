using System;

namespace Inter.Helpers
{
    public static class ConstHelper
    {
        public const string DefaultImageUrl = "_system/images/no_img.png";
        public const string DefaultSmallImageUrl = "_system/images/no_img_sl.png";
        public const string DefaultNormalImageUrl = "_system/images/no_img_nm.png";
        public const string DefaultLargeImageUrl = "_system/images/no_img_lg.png";
        public const string DefaultFilePicUrl = "_system/images/file_pic.png";
        public const string TempFolderUrl = "imgbrd/tmp";
        public const string LoginPath = "/Account/Login";
        
        public const string FilesFolderName = "files";
        public const string CompressedImageFolderName = "comp_img";
        public const string RandomThreadName = "Случайный тред";
        
        public const int MaxTextLength = 35000;
        public const int MaxNameLength = 50;
        public const int MaxFilesCount = 8;
        public const int MaxFileNameLength = 64;
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
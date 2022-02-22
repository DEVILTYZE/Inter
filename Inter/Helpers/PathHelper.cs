using Inter.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inter.Helpers
{
    public class PathHelper
    {
        private readonly IUrlHelper _url;

        private readonly string[] _avatarSizes = { "/small.jpg", "/normal.jpg" };

        public PathHelper(IUrlHelper url) => _url = url;

        public string GetFilePath(Thread thread, string fileName) 
            => FileHelper.IsImage(fileName)
                ? _url.Content($"~/files/{thread.FileFolderUrl}/{fileName}")
                : _url.Content($"~/files/{ConstHelper.DefaultFilePicUrl}");

        public string GetFilePath(User user)
            => _url.Content($"~/files/{user.AvatarUrl}");
        
        public string GetFilePath(Board board)
            => string.IsNullOrEmpty(board.ImageUrl) 
                ? _url.Content($"~/files/{ConstHelper.DefaultNormalImageUrl}") 
                : _url.Content($"~/files/{board.ImageUrl}");
        
        public string GetCompressedFilePath(Thread thread, string fileName)
            => FileHelper.IsImage(fileName)
                ? _url.Content($"~/files/{thread.FileFolderUrl}/{ConstHelper.CompressedImageFolderName}/{fileName}")
                : _url.Content($"~/files/{ConstHelper.DefaultFilePicUrl}");

        public string GetCompressedFilePath(User user, bool isSmall = true) 
            => GetCompressedFilePath(user.AvatarUrl, isSmall);

        public string GetCompressedFilePath(string path, bool isSmall = true)
        {
            var indexOfSize = isSmall ? 0 : 1;
            var defaultPath = isSmall ? ConstHelper.DefaultSmallImageUrl : ConstHelper.DefaultNormalImageUrl;

            return string.IsNullOrEmpty(path) 
                ? _url.Content($"~/files/{defaultPath}")
                : _url.Content($"~/files/{path[..path.LastIndexOf('/')]}{_avatarSizes[indexOfSize]}");
        }

        public static string GetThreadFolderPath(string boardId, string threadId) 
            => $"imgbrd/board_{boardId}/thread_{threadId}";
    }
}
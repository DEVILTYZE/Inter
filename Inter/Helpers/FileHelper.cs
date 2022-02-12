using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inter.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Inter.Helpers
{
    public static class FileHelper
    {
        private const int MaxSmallAvatarSize = 50;
        private const int MaxNormalAvatarSize = 350;
        private const int MaxNormalPreviewPicSize = 250;

        // РЕШИТЬ ПРОБЛЕМУ С МЕЛКИМИ КАРТИНКАМИ (ГОТОВО)
        // РЕШИТЬ ПРОБЛЕМУ С ФАЙЛАМИ (ГОТОВО)
        // РЕШИТЬ ПРОБЛЕМУ С ОТОБРАЖЕНИЕМ ПОСТОВ
        
        public static async Task SaveFileAsync(IFormFile formFile, string path, IWebHostEnvironment environment)
        {
            if (IsImage(formFile.FileName))
            {
                var lastIndex = path.LastIndexOf('/');
                var fileName = path[(lastIndex + 1)..].Replace('/', '\\');
                var pathDir = path[..lastIndex].Replace('/', '\\');
                var pathDirCompressedImage = Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, pathDir,
                    ConstHelper.CompressedImageFolderName);
                Directory.CreateDirectory(pathDirCompressedImage);
                var compressedImage = CropImage(
                    CompressImage(new Bitmap(formFile.OpenReadStream()), MaxNormalPreviewPicSize), MaxNormalPreviewPicSize);

                await using var fs = new FileStream(Path.Combine(pathDirCompressedImage, fileName), FileMode.Create);
                compressedImage.Save(fs, ImageFormat.Png);
            }
            
            await using (var fs = new FileStream(Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, path), FileMode.Create))
            {
                await formFile.CopyToAsync(fs);
            }
        }

        // fileFolderUrl = thread.fileFolderUrl
        public static List<string> ReplaceFiles(IEnumerable<string> paths, string fileFolderUrl, IWebHostEnvironment environment)
        {
            var names = new List<string>();
            
            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path) || path.Length > ConstHelper.MaxFileNameLength)
                    continue;

                FileInfo fInfo;

                try
                {
                    fInfo = new FileInfo(path);
                }
                catch (Exception)
                {
                    continue;
                }

                var pathIndex = path.LastIndexOf('/');
                
                if (pathIndex < 0)
                    continue;
                
                var compressedPath = Path.Combine(path[..pathIndex], ConstHelper.CompressedImageFolderName, fInfo.Name);
                var replacedPath = $"{fileFolderUrl}/{fInfo.Name}";
                var replacedCompressedPath = $"{fileFolderUrl}/{ConstHelper.CompressedImageFolderName}/{fInfo.Name}";
                var dirInfo = new DirectoryInfo(Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName,
                    replacedCompressedPath[..replacedCompressedPath.LastIndexOf('/')].Replace('/', '\\')));
                
                if (!dirInfo.Exists)
                    dirInfo.Create();

                var fullCompressedPath = Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, compressedPath.Replace('/', '\\'));
                var fullPath = Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, path.Replace('/', '\\'));
                
                if (IsImage(path) && File.Exists(fullCompressedPath))
                    File.Move(fullCompressedPath, Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, 
                        replacedCompressedPath.Replace('/', '\\')));
                
                if (File.Exists(fullPath))
                    File.Move(fullPath, Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, 
                        replacedPath.Replace('/', '\\')));
                
                names.Add(fInfo.Name);
            }

            return names;
        }
        
        // public static async Task SaveBoardImageAsync(IFormFile formFile, Board board, IWebHostEnvironment environment)
        // {
        //     if (!IsImage(formFile.FileName))
        //         return;
        //
        //     var fileName = GetNewFileName(formFile);
        //     var path = $"imgbrd/board_{board.Id}/_main/{fileName}";
        //     var pathDir = Path.Combine(environment.WebRootPath, $"\\files\\imgbrd\\board_{board.Id}\\_main");
        //     Directory.CreateDirectory(pathDir);
        //     board.ImageUrl = path;
        //     
        //     await using var fs = new FileStream(Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, path), FileMode.Create);
        //     await formFile.CopyToAsync(fs);
        // }

        public static async Task<string> SaveAccountImageAsyncOrDefault(IFormFile formFile, string userId, string fileName, 
            IWebHostEnvironment environment)
        {
            if (!IsImage(formFile.FileName))
                return null;

            var path = $"accnt/user_{userId}/{fileName}";
            var pathDir = environment.WebRootPath + $"\\files\\accnt\\user_{userId}";
            
            if (!Directory.Exists(pathDir))
                Directory.CreateDirectory(pathDir);
            
            if (File.Exists(Path.Combine(pathDir, fileName)))
                return null;
            
            var compressedSmallImage = CropImage(
                CompressImage(new Bitmap(formFile.OpenReadStream()), MaxSmallAvatarSize), MaxSmallAvatarSize);
            var compressedNormalImage = CropImage(
                CompressImage(new Bitmap(formFile.OpenReadStream()), MaxNormalAvatarSize), MaxNormalAvatarSize);

            await using(var fs = new FileStream(Path.Combine(pathDir, "small.jpg"), FileMode.Create))
            {
                compressedSmallImage.Save(fs, ImageFormat.Jpeg);
            }

            await using(var fs = new FileStream(Path.Combine(pathDir, "normal.jpg"), FileMode.Create))
            {
                compressedNormalImage.Save(fs, ImageFormat.Jpeg);
            }
            
            await using(var fs = new FileStream(Path.Combine(pathDir, fileName), FileMode.Create))
            {
                await formFile.CopyToAsync(fs);
            }

            return path;
        }

        public static void UpdateFilePathsUser(string filePathString, User user, IWebHostEnvironment environment)
        {
            if (File.Exists(Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, filePathString)))
                user.AvatarUrl = filePathString;
        }

        public static void RemoveFilesFolder(string path, IWebHostEnvironment environment) 
            => Directory.Delete(Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, path), true);

        public static void RemoveFiles(IEnumerable<string> filePaths, IWebHostEnvironment environment)
        {
            foreach (var filePath in filePaths.Where(filePath => !string.IsNullOrEmpty(filePath)))
            {
                var path = Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName, filePath);
                
                if (File.Exists(path))
                    File.Delete(path);

                var lastIndex = filePath.LastIndexOf('/');
                var compressedPath = Path.Combine(environment.WebRootPath, ConstHelper.FilesFolderName,
                    filePath[..lastIndex], ConstHelper.CompressedImageFolderName, filePath[(lastIndex + 1)..]);
                
                if (File.Exists(compressedPath))
                    File.Delete(compressedPath);
            }
        }

        public static string GetNewFileName(IFormFile file)
        {
            var time = DateTime.Now;
            var fileInfo = new FileInfo(file.FileName);

            return GenerateRandomWord() + time.Hour + time.Minute + time.Second + time.Millisecond + fileInfo.Extension;
        }
        
        public static bool IsImage(string filePath)
        {
            var imageExtensions = new[] { "PNG", "JPG", "JPEG", "GIF", "BMP", "RAW", "TIFF" };
            var fileInfo = new FileInfo(filePath);
            var extension = fileInfo.Extension;

            return imageExtensions.Any(imageExtension 
                => string.CompareOrdinal(extension[1..].ToUpper(), imageExtension) == 0);
        }

        public static bool IsNormalFileSize(long fileSize) => ConstHelper.MaxFileSize >= fileSize;

        private static Bitmap CompressImage(Image image, double neededMaxHeight)
        {
            var compressionRatio = image.Height / neededMaxHeight;
            var height = Convert.ToInt32(image.Height / compressionRatio);
            var width = Convert.ToInt32(image.Width / compressionRatio);

            var rectangle = new Rectangle(0, 0, width, height);
            var bitmap = new Bitmap(width, height);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(bitmap);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return bitmap;
        }

        private static Bitmap CropImage(Bitmap pic, int cropSize)
        {
            if (pic.Height == pic.Width)
                return pic;

            var size = new Size(cropSize, cropSize);
            var point = pic.Width == cropSize
                ? new Point(0, pic.Height / 2 - cropSize / 2)
                : new Point(pic.Width / 2 - cropSize / 2, 0);
            var rect = new Rectangle(point, size);

            return pic.Clone(rect, pic.PixelFormat);
        }

        private static string GenerateRandomWord()
        {
            const int length = 8;
            
            var random = new Random();
            var sb = new StringBuilder();

            for (var i = 0; i < length; ++i)
                sb.Append((char)('a' + random.Next(0, 26)));

            return sb.ToString();
        }
    }
}
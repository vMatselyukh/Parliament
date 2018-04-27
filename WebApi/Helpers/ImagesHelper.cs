using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using WebApi.Dtos;
using WebApi.Enums;

namespace WebApi.Helpers
{
    public class ImagesHelper
    {
        public static string SaveImageForPolitician(HttpPostedFileBase image, PersonDto personDto, PoliticianImageTypeEnum imageType)
        {
            var fileName = Path.Combine(ConfigHelper.ContentPath, ConfigHelper.POLITICIANS_FOLDER, personDto.GenericName, 
                $"{personDto.GenericName}{imageType.ToString()}{Path.GetExtension(image.FileName)}");

            var rescueFileName = Path.Combine(ConfigHelper.ContentPath, ConfigHelper.POLITICIANS_FOLDER, personDto.GenericName,
                $"{personDto.GenericName}{imageType.ToString()}Rescue{Path.GetExtension(image.FileName)}");

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            try
            {
                image.SaveAs(fileName);
            }
            catch
            {
                fileName = rescueFileName;
                image.SaveAs(fileName);
            }

            var relativeFilePath = fileName.Replace(ConfigHelper.ContentPath, "");
            var pathPlusContent = "\\content" + relativeFilePath;

            var imageInfo = new ImageInfoDto
            {
                ImagePath = pathPlusContent,
                Md5Hash = FileHelper.CalcMD5(fileName)
            };

            switch (imageType)
            {
                case PoliticianImageTypeEnum.MainPic:
                    personDto.MainPicPath = imageInfo;
                    break;

                case PoliticianImageTypeEnum.ListButtonPic:
                    personDto.ListButtonPicPath = imageInfo;
                    break;

                case PoliticianImageTypeEnum.SmallButtonPic:
                    personDto.SmallButtonPicPath = imageInfo;
                    break;
            }

            return pathPlusContent;
        }

        public static void RemoveOldImage(string oldImagePath)
        {
            if (string.IsNullOrEmpty(oldImagePath))
            {
                throw new Exception("OldImagePath can't be empty.");
            }

            var pathElements = oldImagePath.Split(Path.DirectorySeparatorChar).ToList();
            pathElements.Remove("content");

            if (File.Exists(Path.Combine(ConfigHelper.ContentPath, Path.Combine(pathElements.ToArray()))))
            {
                File.Delete(Path.Combine(ConfigHelper.ContentPath, Path.Combine(pathElements.ToArray())));
            }
        }

        public static Dimensions GetImageDimensions(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new Exception("Image can't have empty path.");
            }

            var pathElements = imagePath.Split(Path.DirectorySeparatorChar).ToList();
            pathElements.Remove("content");

            if (!File.Exists(Path.Combine(ConfigHelper.ContentPath, Path.Combine(pathElements.ToArray()))))
            {
                return new Dimensions
                {
                    Width = 0,
                    Height = 0
                };
            }

            var img = Image.FromFile(Path.Combine(ConfigHelper.ContentPath, Path.Combine(pathElements.ToArray())));

            return new Dimensions
            {
                Height = img.Height,
                Width = img.Width
            };
        }
    }
}
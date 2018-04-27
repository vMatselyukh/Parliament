using WebApi.Dtos;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Mappers
{
    public class ImageInfoMapper
    {
        public static ImageInfoDto MapImageInfo(ImageInfo imageInfo)
        {
            if (imageInfo == null)
            {
                return new ImageInfoDto();
            }

            return new ImageInfoDto
            {
                ImagePath = imageInfo.ImagePath,
                Md5Hash = imageInfo.Md5Hash,
                Dimensions = ImagesHelper.GetImageDimensions(imageInfo.ImagePath)
            };
        }

        public static ImageInfo MapImageInfoDto(ImageInfoDto imageInfoDto)
        {
            if (imageInfoDto == null)
            {
                return new ImageInfo();
            }

            return new ImageInfo
            {
                ImagePath = imageInfoDto.ImagePath,
                Md5Hash = imageInfoDto.Md5Hash
            };
        }
    }
}
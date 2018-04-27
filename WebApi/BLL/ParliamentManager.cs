using System.Web;
using WebApi.Dtos;
using WebApi.Enums;
using WebApi.Helpers;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.BLL
{
    public class ParliamentManager
    {
        public PersonDto FillImagesInfo(HttpPostedFileBase image, PoliticianImageTypeEnum imageType, PersonDto personDto)
        {
            if (image != null)
            {
                var imagePath = ImagesHelper.SaveImageForPolitician(image, personDto, imageType);
            }

            return personDto;
        }

        public PersonDto FillImages(HttpPostedFileBase mainPic, HttpPostedFileBase listButtonPic, HttpPostedFileBase smallButtonPic, 
            PersonDto personDto, Person person)
        {
            personDto.MainPicPath = ImageInfoMapper.MapImageInfo(person.MainPicPath);
            personDto.ListButtonPicPath = ImageInfoMapper.MapImageInfo(person.ListButtonPicPath);
            personDto.SmallButtonPicPath = ImageInfoMapper.MapImageInfo(person.SmallButtonPicPath);

            FillImagesInfo(mainPic, PoliticianImageTypeEnum.MainPic, personDto);
            FillImagesInfo(listButtonPic, PoliticianImageTypeEnum.ListButtonPic, personDto);
            FillImagesInfo(smallButtonPic, PoliticianImageTypeEnum.SmallButtonPic, personDto);

            if (personDto.MainPicPath != null && !string.IsNullOrEmpty(personDto.MainPicPath.ImagePath))
            {
                personDto.MainPicPath.Dimensions = ImagesHelper.GetImageDimensions(personDto.MainPicPath.ImagePath);
            }

            if (personDto.ListButtonPicPath != null && !string.IsNullOrEmpty(personDto.ListButtonPicPath.ImagePath))
            {
                personDto.ListButtonPicPath.Dimensions = ImagesHelper.GetImageDimensions(personDto.ListButtonPicPath.ImagePath);
            }

            if (personDto.SmallButtonPicPath != null && !string.IsNullOrEmpty(personDto.SmallButtonPicPath.ImagePath))
            {
                personDto.SmallButtonPicPath.Dimensions = ImagesHelper.GetImageDimensions(personDto.SmallButtonPicPath.ImagePath);
            }

            return personDto;
        }

        public TrackDto FillAudioFile(HttpPostedFileBase audioFile, TrackDto trackDto)
        {
            if (audioFile != null)
            {
                trackDto.AudioFilePath = AudioHelper.SaveAudioForTrackForPolitician(audioFile, trackDto);
            }
            return trackDto;
        }
    }
}
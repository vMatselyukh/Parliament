using System.Collections.Generic;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Mappers
{
    public class PersonMapper
    {
        public static PersonDto MapPerson(Person person) {
            return new PersonDto
            {
                Id = person.Id,
                GenericName = person.Name,
                GenericPost = person.Post,
                ListButtonPicPath = ImageInfoMapper.MapImageInfo(person.ListButtonPicPath),
                MainPicPath = ImageInfoMapper.MapImageInfo(person.MainPicPath),
                SmallButtonPicPath = ImageInfoMapper.MapImageInfo(person.SmallButtonPicPath),
                Tracks = person.Tracks == null ? new List<Track>() : person.Tracks
            };
        }

        public static Person MapPersonDto(PersonDto personDto)
        {
            return new Person
            {
                Id = personDto.Id,
                Name = personDto.GenericName,
                Post = personDto.GenericPost,
                ListButtonPicPath = ImageInfoMapper.MapImageInfoDto(personDto.ListButtonPicPath),
                MainPicPath = ImageInfoMapper.MapImageInfoDto(personDto.MainPicPath),
                SmallButtonPicPath = ImageInfoMapper.MapImageInfoDto(personDto.SmallButtonPicPath),
                Tracks = personDto.Tracks
            };
        }
    }
}
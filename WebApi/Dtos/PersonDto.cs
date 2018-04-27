using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string GenericName { get; set; }
        public string UkranianName { get; set; }
        public string RussianName { get; set; }
        public string UkranianPost { get; set; }
        public string RussianPost { get; set; }
        public string GenericPost { get; set; }
        public List<Track> Tracks { get; set; }
        public ImageInfoDto MainPicPath { get; set; }
        public ImageInfoDto SmallButtonPicPath { get; set; }
        public ImageInfoDto ListButtonPicPath { get; set; }

        public PersonDto()
        {
            Tracks = new List<Track>();
            MainPicPath = new ImageInfoDto();
            SmallButtonPicPath = new ImageInfoDto();
            ListButtonPicPath = new ImageInfoDto();
        }
    }
}
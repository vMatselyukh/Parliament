namespace WebApi.Dtos
{
    public class ImageInfoDto
    {
        public string ImagePath { get; set; }
        public string Md5Hash { get; set; }
        public Dimensions Dimensions { get; set; }
    }
}
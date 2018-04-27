using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public ImageInfo MainPicPath { get; set; }
        public ImageInfo SmallButtonPicPath { get; set; }
        public ImageInfo ListButtonPicPath { get; set; }
        public List<Track> Tracks { get; set; }
        public Person()
        {
            Tracks = new List<Track>();
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (Tracks == null) Tracks = new List<Track>();
        }
    }
}
using System;

namespace WebApi.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Rating { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? Date { get; set; }
        public string Md5Hash { get; set; }
    }
}
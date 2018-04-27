using System;

namespace WebApi.Dtos
{
    public class TrackDto
    {
        public int Id { get; set; }
        public int PoliticianId { get; set; }
        public DateTime? Date { get; set; }
        public string PoliticianName { get; set; }
        public string Name { get; set; }
        public string AudioFilePath { get; set; }
        public string Md5Hash { get; set; }
    }
}
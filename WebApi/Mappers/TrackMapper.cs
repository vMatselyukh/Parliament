using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Mappers
{
    public class TrackMapper
    {
        public static TrackDto MapTrack(Track track)
        {
            return new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Date = track.Date,
                AudioFilePath = track.Path,
                Md5Hash = track.Md5Hash
            };
        }

        public static Track MapTrackDto(TrackDto trackDto)
        {
            return new Track
            {
                Id = trackDto.Id,
                Name = trackDto.Name,
                Date = trackDto.Date,
                Path = trackDto.AudioFilePath,
                Md5Hash = trackDto.Md5Hash
            };
        }
    }
}
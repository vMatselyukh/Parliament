using System.IO;
using System.Web;
using WebApi.Dtos;

namespace WebApi.Helpers
{
    public class AudioHelper
    {
        public static string SaveAudioForTrackForPolitician(HttpPostedFileBase audioFile, TrackDto trackDto)
        {
            var fileName = Path.Combine(ConfigHelper.ContentPath, ConfigHelper.TRACKS_FOLDER, trackDto.PoliticianName, $"{audioFile.FileName}");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            audioFile.SaveAs(fileName);

            trackDto.Md5Hash = FileHelper.CalcMD5(fileName);

            var relativeFilePath = fileName.Replace(ConfigHelper.ContentPath, "");
            var pathPlusContent = "\\content" + relativeFilePath;

            return pathPlusContent;
        }
    }
}
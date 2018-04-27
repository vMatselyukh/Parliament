using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebApi.Models;

namespace WebApi.Helpers
{
    public class FileHelper
    {
        public static void DeleteNotNeededFiles()
        {
            var config = ConfigHelper.GetConfig();
            List<string> usedFiles = new List<string>();

            config.Persons.ForEach(person =>
            {
                if (person.MainPicPath != null)
                {
                    usedFiles.Add(person.MainPicPath.ImagePath);
                }

                if (person.ListButtonPicPath != null)
                {
                    usedFiles.Add(person.ListButtonPicPath.ImagePath);
                }

                if (person.SmallButtonPicPath != null)
                {
                    usedFiles.Add(person.SmallButtonPicPath.ImagePath);
                }

                person.Tracks.ForEach(track =>
                {
                    usedFiles.Add(track.Path);                    
                });
            });

            usedFiles = usedFiles.Where(usedFile => !string.IsNullOrEmpty(usedFile)).Distinct().ToList();
            var allFiles = GetImagesAndTracksInWholeApp();
            var unusedFiles = allFiles.Where(file => !usedFiles.Any(usedFile => file.ToLower().Contains(usedFile.ToLower()))).ToList();
            unusedFiles.ForEach(unusedFile => DeleteFile(unusedFile));
        }

        public static string CalcMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.AppendFormat("{0:x2}", hash[i]);
                    }

                    return sb.ToString();
                }
            }
        }

        private static void DeleteFile(string fileToRemovePath)
        {
            if (string.IsNullOrEmpty(fileToRemovePath))
            {
                return;
            }

            if (File.Exists(fileToRemovePath))
            {
                File.Delete(fileToRemovePath);
            }
        }

        private static List<string> GetImagesAndTracksInWholeApp()
        {
            var politiciansImages = Directory.GetFiles(Path.Combine(ConfigHelper.ContentPath, ConfigHelper.POLITICIANS_FOLDER), "*", SearchOption.AllDirectories);
            var tracks = Directory.GetFiles(Path.Combine(ConfigHelper.ContentPath, ConfigHelper.TRACKS_FOLDER), "*", SearchOption.AllDirectories);

            var filesList = politiciansImages.ToList();
            filesList.AddRange(tracks.ToList());

            return filesList;
        }
    }
}
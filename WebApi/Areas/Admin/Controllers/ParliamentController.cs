using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.BLL;
using WebApi.Dal.Repositories;
using WebApi.Dtos;
using WebApi.Helpers;
using WebApi.Mappers;
using WebApi.Models;

namespace WebApi.Areas.Admin.Controllers
{
    public class ParliamentController : Controller
    {
        // GET: Admin/Parliament
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Parliament
        public ActionResult Politicians()
        {
            var config = ConfigHelper.GetConfig();
            return View(config);
        }

        [HttpGet]
        public ActionResult Politician(int id = 0)
        {
            if (id == 0)
            {
                return View(new PersonDto());
            }

            var personRepo = new PersonRepository();
            var person = personRepo.Get(id);
            
            var personDto = PersonMapper.MapPerson(person);

            var languageManager = new LanguageManager();
            personDto = languageManager.GetTranslations(personDto);
            return View(personDto);
        }

        [HttpPost]
        public ActionResult Politician(HttpPostedFileBase mainPic, HttpPostedFileBase listButtonPic, HttpPostedFileBase smallButtonPic, PersonDto personDto)
        {
            var personRepo = new PersonRepository();
            var languageManager = new LanguageManager();
            var parliamentManager = new ParliamentManager();
            Person person;

            if (personDto.Id == 0)
            {
                if (personDto.GenericName.Trim() == personDto.GenericPost.Trim())
                {
                    TempData["Notifications"] = $"Please select different strings for post and name.";
                    return View("Politician", personDto);
                }

                if (LanguageHelper.IsInUse(personDto.GenericName))
                {
                    TempData["Notifications"] = $"Name {personDto.GenericName} is already used in dictionary. Please select another one.";
                    return View("Politician", personDto);
                }

                if (LanguageHelper.IsInUse(personDto.GenericPost))
                {
                    TempData["Notifications"] = $"Post {personDto.GenericPost} is already used in dictionary. Please select another one.";
                    return View("Politician", personDto);
                }

                person = new Person();
                var firstPersonInList = personRepo.GetAll().OrderByDescending(p => p.Id).FirstOrDefault();
                if (firstPersonInList == null)
                {
                    personDto.Id = 1;
                }
                else
                {
                    personDto.Id = firstPersonInList.Id + 1;
                }

                TempData["Notifications"] = "Added new politician...";
            }
            else {
                person = personRepo.Get(personDto.Id);
                TempData["Notifications"] = "Saved...";
            }

            parliamentManager.FillImages(mainPic, listButtonPic, smallButtonPic, personDto, person);
            
            personDto.Tracks = person.Tracks;
            var updatedPerson = PersonMapper.MapPersonDto(personDto);

            languageManager.SaveTranslations(personDto);
            personRepo.Upsert(updatedPerson);

            return RedirectToAction("Politician", new { id = personDto.Id });
        }

        [HttpPost]
        public ActionResult DeletePolitician(int id)
        {
            var personRepo = new PersonRepository();
            var languageManager = new LanguageManager();

            var person = personRepo.Get(id);

            languageManager.DeleteTranslations(person.Name);
            languageManager.DeleteTranslations(person.Post);

            personRepo.Delete(id);

            ViewBag.Notifications = $"Person {person.Name} deleted.";

            return View("Politicians", ConfigHelper.GetConfig());
        }
        
        [HttpPost]
        public ActionResult DeleteTrack(int politicianId, int trackId)
        {
            var personRepo = new PersonRepository();
            var languageManager = new LanguageManager();

            var deletedTrack = personRepo.GetTrack(politicianId, trackId);
            personRepo.DeleteTrack(politicianId, trackId);

            ViewBag.Notifications = $"Track {deletedTrack.Name} deleted.";

            var person = personRepo.Get(politicianId);

            var personDto = PersonMapper.MapPerson(person);
            personDto = languageManager.GetTranslations(personDto);

            return View("Politician", personDto);
        }

        [HttpGet]
        public ActionResult Track(int politicianId, int trackId = 0)
        {
            var config = ConfigHelper.GetConfig();
            var person = config.Persons.FirstOrDefault(p => p.Id == politicianId);
            if (person == null)
            {
                ViewBag.Error = "Person wasn't found";
                Response.RedirectToRoute("/Error");
                return null;
            }

            var neededTrack = new Track();
            if (trackId != 0)
            {
                neededTrack = person.Tracks.FirstOrDefault(track => track.Id == trackId);
            }
                       
            var trackDto = TrackMapper.MapTrack(neededTrack);
            trackDto.PoliticianId = person.Id;
            trackDto.PoliticianName = person.Name;
            return View(trackDto);
        }

        [HttpPost]
        public ActionResult Track(HttpPostedFileBase audioFile, TrackDto trackDto)
        {
            var personRepo = new PersonRepository();
            var parliamentManager = new ParliamentManager();
            var person = personRepo.GetAll().FirstOrDefault(p => p.Id == trackDto.PoliticianId);
            if (person == null)
            {
                ViewBag.Error = "Person wasn't found";
                Response.RedirectToRoute("/Error");
                return null;
            }

            trackDto.PoliticianName = person.Name;
            trackDto = parliamentManager.FillAudioFile(audioFile, trackDto);
            var trackToUpdate = TrackMapper.MapTrackDto(trackDto);
            var updatedTrack = personRepo.UpsertTrack(trackDto.PoliticianId, trackToUpdate);

            TempData["Notifications"] = "Added...";
            return RedirectToAction("Track", new { politicianId = person.Id, trackId = updatedTrack.Id });
        }
    }
}
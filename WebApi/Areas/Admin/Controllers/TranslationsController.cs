using System.Web.Mvc;
using WebApi.BLL;
using WebApi.Dtos;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.Areas.Admin.Controllers
{
    public class TranslationsController : Controller
    {
        // GET: Admin/Translations
        public ActionResult Index()
        {
            return View(LanguageHelper.GetBaseTranslationDtos());
        }

        [HttpPost]
        public ActionResult DeleteTranslation(string key)
        {
            LanguageHelper.DeleteTranslation(key, TranslationsTypeEnum.Base);

            TempData["Notifications"] = $"Deleted translation with key {key}.";

            return RedirectToAction("Index");
        }

        public ActionResult Wording(string key)
        {            
            var model = new TranslationDto()
            {
                IsRemovable = true,
                Translations = new System.Collections.Generic.Dictionary<LanguageEnum, string>
                {
                    { LanguageEnum.Russian, ""},
                    { LanguageEnum.Ukrainian, ""}
                }
            };

            if (string.IsNullOrEmpty(key))
            {
                ViewBag.AddNew = true;
                return View(model);
            }

            var translation = LanguageHelper.GetBaseTranslationDtoByKey(key);

            if (translation != null)
            {
                return View(translation);
            }

            return Redirect("Error");
        }

        [HttpPost]
        public ActionResult Wording(FormCollection form, EditTranslationDto editTranslationDto, bool? addNew)
        {
            var translationDto = editTranslationDto.TranslationDto;
            translationDto.Key = translationDto.Key.Trim();

            var translation = LanguageHelper.GetBaseTranslationDtoByKey(translationDto.Key);
            if (translation != null)
            {
                //-----when we are trying to add new key that is already in use.
                if (addNew == true)
                {
                    ViewBag.Notifications = "Such key is already in use.";

                    return View(translation);
                }

                LanguageHelper.SaveTranslationDto(translationDto, TranslationsTypeEnum.Base);
            }
            else
            {
                var translationWithOldKey = LanguageHelper.GetBaseTranslationDtoByKey(editTranslationDto.OldKey);
                if (translationWithOldKey != null && translationWithOldKey.IsRemovable)
                {
                    LanguageHelper.DeleteTranslation(editTranslationDto.OldKey, TranslationsTypeEnum.Base);
                }
                LanguageHelper.SaveTranslationDto(translationDto, TranslationsTypeEnum.Base);                
            }

            ViewBag.Notifications = "Saved...";

            return View(translationDto);
        }
    }
}
using System;
using WebApi.Dtos;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.BLL
{
    public class LanguageManager
    {
        public PersonDto GetTranslations(PersonDto personDto)
        {
            personDto.RussianName = LanguageHelper.GetTranslationByLanguageAndKeyAnyType(LanguageEnum.Russian, personDto.GenericName, TranslationsTypeEnum.Politicians);
            personDto.UkranianName = LanguageHelper.GetTranslationByLanguageAndKeyAnyType(LanguageEnum.Ukrainian, personDto.GenericName, TranslationsTypeEnum.Politicians);

            personDto.RussianPost = LanguageHelper.GetTranslationByLanguageAndKeyAnyType(LanguageEnum.Russian, personDto.GenericPost, TranslationsTypeEnum.Politicians);
            personDto.UkranianPost = LanguageHelper.GetTranslationByLanguageAndKeyAnyType(LanguageEnum.Ukrainian, personDto.GenericPost, TranslationsTypeEnum.Politicians);

            return personDto;
        }

        public void SaveTranslations(PersonDto personDto)
        {
            LanguageHelper.SaveTranslation(LanguageEnum.Russian, personDto.GenericName, personDto.RussianName, TranslationsTypeEnum.Politicians);
            LanguageHelper.SaveTranslation(LanguageEnum.Russian, personDto.GenericPost, personDto.RussianPost, TranslationsTypeEnum.Politicians);

            LanguageHelper.SaveTranslation(LanguageEnum.Ukrainian, personDto.GenericName, personDto.UkranianName, TranslationsTypeEnum.Politicians);
            LanguageHelper.SaveTranslation(LanguageEnum.Ukrainian, personDto.GenericPost, personDto.UkranianPost, TranslationsTypeEnum.Politicians);
        }

        public void DeleteTranslations(string key)
        {
            LanguageHelper.DeleteTranslation(LanguageEnum.Russian, key, TranslationsTypeEnum.Politicians);
            LanguageHelper.DeleteTranslation(LanguageEnum.Ukrainian, key, TranslationsTypeEnum.Politicians);
        }
    }
}
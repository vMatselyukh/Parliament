using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using WebApi.Dtos;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Dal.Repositories;

namespace WebApi.Helpers
{
    public class LanguageHelper
    {
        private static string translationsUAKey = "TranslationsUAFilePath";
        private static string politiciansTranslationsUAKey = "PoliticiansTranslationsUAFilePath";
        private static string translationsRUKey = "TranslationsRUFilePath";
        private static string politiciansTranslationsRUKey = "PoliticiansTranslationsRUFilePath";

        public static List<TranslationDto> GetBaseTranslationDtos()
        {
            return GetTranslationDtos(translationsUAKey, translationsRUKey);
        }

        public static List<TranslationDto> GetPoliticiansTranslationDtos()
        {
            return GetTranslationDtos(politiciansTranslationsUAKey, politiciansTranslationsRUKey);
        }

        public static List<TranslationDto> GetTranslationDtos(TranslationsTypeEnum translationType)
        {
            if (translationType == TranslationsTypeEnum.Base)
            {
                return GetBaseTranslationDtos();
            }

            return GetPoliticiansTranslationDtos();
        }

        public static List<TranslationDto> GetTranslationDtos(string uaKey, string ruKey)
        {
            var translations = new TranslationDto();
            Dictionary<string, string> uaTranslations = GetTranslationsByFilePath(ConfigurationManager.AppSettings[uaKey]);
            Dictionary<string, string> ruTranslations = GetTranslationsByFilePath(ConfigurationManager.AppSettings[ruKey]);
            var personRepo = new PersonRepository();
            var politicians = personRepo.GetAll();

            if (uaTranslations.Count != ruTranslations.Count)
            {
                throw new Exception("Translations count doesn't match.");
            }

            var translationDtos = new List<TranslationDto>();

            foreach (var translation in uaTranslations)
            {
                if (!ruTranslations.ContainsKey(translation.Key))
                {
                    throw new Exception($"Russian dictionary doesn't contain key {translation.Key}.");
                }

                translationDtos.Add(new TranslationDto
                {
                    Key = translation.Key,
                    IsRemovable = IsRemovable(politicians, translation.Key),
                    Translations = new Dictionary<LanguageEnum, string>
                    {
                        { LanguageEnum.Ukrainian, translation.Value },
                        { LanguageEnum.Russian, ruTranslations[translation.Key] }
                    }
                });
            }

            return translationDtos;
        }

        private static bool IsRemovable(IList<Person> politicians, string key)
        {
            return !politicians.Any(politician => politician.Name == key || politician.Post == key);
        }

        public static bool IsInUse(string key)
        {
            var politicianTranslations = GetPoliticiansTranslationDtos();
            var baseTranslations = GetBaseTranslationDtos();

            return politicianTranslations.Any(translation=>translation.Key == key) 
                || baseTranslations.Any(translation => translation.Key == key);
        }

        public static Dictionary<string, string> GetTranslationsByLanguageAndType(LanguageEnum language, TranslationsTypeEnum translationType)
        {
            var translations = new Dictionary<string, string>();

            string configKey = string.Empty;

            switch (language)
            {
                case LanguageEnum.Ukrainian:
                    if (translationType == TranslationsTypeEnum.Base)
                    {
                        configKey = translationsUAKey;
                    }
                    else
                    {
                        configKey = politiciansTranslationsUAKey;
                    }
                    break;
                case LanguageEnum.Russian:
                    if (translationType == TranslationsTypeEnum.Base)
                    {
                        configKey = translationsRUKey;
                    }
                    else
                    {
                        configKey = politiciansTranslationsRUKey;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(configKey))
            {
                throw new Exception($"No language resource file found for language: {language.ToString()}.");
            }

            return GetTranslationsByFilePath(ConfigurationManager.AppSettings[configKey]);
        }

        private static Dictionary<string, string> GetTranslationsByFilePath(string filePath)
        {
            var translationsPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{filePath}";
            if (File.Exists(translationsPath))
            {
                string translationsString = File.ReadAllText(translationsPath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(translationsString);
            }
            else
            {
                throw new Exception("TranslationsUAFilePath doesn't exist.");
            }
        }

        public static string GetTranslationByLanguageAndKeyAnyType(LanguageEnum language, string key, TranslationsTypeEnum translationsType)
        {
            var translations = GetTranslationDtos(translationsType);

            if (translations.Any(translation=>translation.Key == key))
            {
                var translationDto = translations.FirstOrDefault(t => t.Key == key);
                var neededTranslation = translationDto.Translations.FirstOrDefault(t => t.Key == language);
                if (neededTranslation.Equals(default(KeyValuePair<LanguageEnum, string>)))
                {
                    throw new Exception($"There is no translation with key {key} in language {language}.");
                }

                return neededTranslation.Value;
            }

            throw new Exception("Key wasn't found in dictionary.");
        }

        public static TranslationDto GetBaseTranslationDtoByKey(string key)
        {
            return GetTranslationDtoByKey(key, TranslationsTypeEnum.Base);
        }

        public static TranslationDto GetPoliticianTranslationDtoByKey(string key)
        {
            return GetTranslationDtoByKey(key, TranslationsTypeEnum.Politicians);
        }

        public static TranslationDto GetTranslationDtoByKey(string key, TranslationsTypeEnum translationsType)
        {
            if (translationsType == TranslationsTypeEnum.Base)
            {
                return GetBaseTranslationDtos().FirstOrDefault(t => t.Key == key);
            }

            return GetPoliticiansTranslationDtos().FirstOrDefault(t => t.Key == key);
        }

        public static void SaveTranslations(LanguageEnum language, Dictionary<string, string> translations, TranslationsTypeEnum translationType)
        {
            string configKey = string.Empty;

            switch (language)
            {
                case LanguageEnum.Ukrainian:
                    if (translationType == TranslationsTypeEnum.Base)
                    {
                        configKey = translationsUAKey;
                    }
                    else
                    {
                        configKey = politiciansTranslationsUAKey;
                    }
                    break;
                case LanguageEnum.Russian:
                    if (translationType == TranslationsTypeEnum.Base)
                    {
                        configKey = translationsRUKey;
                    }
                    else
                    {
                        configKey = politiciansTranslationsRUKey;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(configKey))
            {
                throw new Exception($"No language resource file found for language: {language.ToString()}.");
            }

            var translationsPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{ConfigurationManager.AppSettings[configKey]}";
            if (File.Exists(translationsPath))
            {
                var translationsJson = JsonConvert.SerializeObject(translations);
                File.WriteAllText(translationsPath, translationsJson);
            }
        }

        public static void SaveTranslation(LanguageEnum language, string key, string value, TranslationsTypeEnum translationType)
        {
            key = key.Trim();
            value = value.Trim();
            var translations = GetTranslationsByLanguageAndType(language, translationType);

            if (translations.ContainsKey(key))
            {
                translations[key] = value;
            }
            else
            {
                translations.Add(key, value);
            }

            SaveTranslations(language, translations, translationType);
        }

        public static void SaveTranslationDto(TranslationDto translationDto, TranslationsTypeEnum translationType)
        {
            var newUkranianTranslation = translationDto.Translations.FirstOrDefault(t => t.Key == LanguageEnum.Ukrainian);
            var newRussianTranslation = translationDto.Translations.FirstOrDefault(t => t.Key == LanguageEnum.Russian);

            if (newUkranianTranslation.Equals(default(KeyValuePair<LanguageEnum, string>))
                || newRussianTranslation.Equals(default(KeyValuePair<LanguageEnum, string>)))
            {
                throw new Exception("There is no translations for one of the languages");
            }

            SaveTranslation(LanguageEnum.Ukrainian, translationDto.Key, newUkranianTranslation.Value, translationType);
            SaveTranslation(LanguageEnum.Russian, translationDto.Key, newRussianTranslation.Value, translationType);
        }

        public static void DeleteTranslation(LanguageEnum language, string key, TranslationsTypeEnum translationType)
        {
            var translations = GetTranslationsByLanguageAndType(language, translationType);

            if (translations.ContainsKey(key))
            {
                translations.Remove(key);
                SaveTranslations(language, translations, translationType);
            }
        }

        public static void DeleteTranslation(string key, TranslationsTypeEnum translationType)
        {
            DeleteTranslation(LanguageEnum.Ukrainian, key, translationType);
            DeleteTranslation(LanguageEnum.Russian, key, translationType);
        }
    }
}
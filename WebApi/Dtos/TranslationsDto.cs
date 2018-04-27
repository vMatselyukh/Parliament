using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Dtos
{
    public class TranslationDto
    {
        public string Key { get; set; }        
        public bool IsRemovable { get; set; }
        public Dictionary<LanguageEnum, string> Translations { get; set; }
        public TranslationDto()
        {
            Translations = new Dictionary<LanguageEnum, string>();
        }
    }
}
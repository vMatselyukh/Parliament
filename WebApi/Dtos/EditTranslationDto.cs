namespace WebApi.Dtos
{
    public class EditTranslationDto
    {
        public string OldKey { get; set; }
        public TranslationDto TranslationDto { get; set; }
    }
}
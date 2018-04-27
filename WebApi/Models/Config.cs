using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebApi.Models
{
    public class Config
    {
        public int CoinsCount { get; set; }
        public List<Person> Persons { get; set; }
        public Config()
        {
            Persons = new List<Person>();
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (Persons == null) Persons = new List<Person>();
        }
    }
}
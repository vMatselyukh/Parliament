using System;
using System.Collections.Generic;
using WebApi.Dal.Interfaces;
using WebApi.Helpers;
using WebApi.Models;
using System.Linq;

namespace WebApi.Dal.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public Person Get(int Id)
        {
            var config = ConfigHelper.GetConfig();
            return config.Persons.First(person=>person.Id == Id);
        }

        public IList<Person> GetAll()
        {
            var config = ConfigHelper.GetConfig();
            return config.Persons;
        }

        public Person Upsert(Person person)
        {
            var config = ConfigHelper.GetConfig();
            try
            {
                var personIndex = config.Persons.FindIndex(p => p.Id == person.Id);
                if (personIndex == -1)
                {
                    config.Persons.Add(person);
                }
                else
                {
                    config.Persons[personIndex] = person;
                }

                ConfigHelper.SaveConfig(config);

                return person;
            }
            catch (Exception e)
            {
                throw new Exception("PersonRepository Upsert failed. " + e.Message);
            }            
        }

        public void Delete(Person person)
        {
            var config = ConfigHelper.GetConfig();
            var personIndex = config.Persons.FindIndex(p => p.Id == person.Id);
            if (personIndex < 0)
            {
                throw new Exception($"PersonRepository Person with index {person.Id}");
            }

            config.Persons.RemoveAt(personIndex);

            ConfigHelper.SaveConfig(config);
        }

        public void Delete(int id)
        {
            var config = ConfigHelper.GetConfig();
            var personIndex = config.Persons.FindIndex(p => p.Id == id);
            if (personIndex < 0)
            {
                throw new Exception($"PersonRepository Person with index {id}");
            }

            config.Persons.RemoveAt(personIndex);

            ConfigHelper.SaveConfig(config);
        }

        public Track GetTrack(int personId, int trackId)
        {
            var config = ConfigHelper.GetConfig();
            var personIndex = config.Persons.FindIndex(p => p.Id == personId);
            if (personIndex < 0)
            {
                throw new Exception($"PersonRepository Person with index {personId}.");
            }

            var trackIndex = config.Persons[personIndex].Tracks.FindIndex(track => track.Id == trackId);
            if (trackIndex < 0)
            {
                throw new Exception($"PersonRepository Track with index {trackId} wasn't found for person with id {personId}.");
            }

            return config.Persons[personIndex].Tracks[trackIndex];
        }

        public Track UpsertTrack(int personId, Track trackToUpdate)
        {
            var config = ConfigHelper.GetConfig();
            var personIndex = config.Persons.FindIndex(p => p.Id == personId);
            if (personIndex < 0)
            {
                throw new Exception($"PersonRepository Person with index {personId}.");
            }

            var person = config.Persons[personIndex];
            var trackIndex = person.Tracks.FindIndex(track => track.Id == trackToUpdate.Id);
            if (trackIndex < 0 && trackToUpdate.Id == 0)
            {
                var trackWithHighestId = person.Tracks.OrderByDescending(track => track.Id).FirstOrDefault();
                trackToUpdate.Id = trackWithHighestId == null ? 1 : trackWithHighestId.Id + 1;
                person.Tracks.Add(trackToUpdate);
            }
            else
            {
                person.Tracks[trackIndex] = trackToUpdate;
            }

            config.Persons[personIndex] = person;

            ConfigHelper.SaveConfig(config);

            trackIndex = trackIndex < 0 ? person.Tracks.FindIndex(track => track.Id == trackToUpdate.Id) : trackIndex;

            return config.Persons[personIndex].Tracks[trackIndex];
        }

        public void DeleteTrack(int personId, int trackId)
        {
            var config = ConfigHelper.GetConfig();
            var personIndex = config.Persons.FindIndex(p => p.Id == personId);
            if (personIndex < 0)
            {
                throw new Exception($"PersonRepository Person with index {personId}.");
            }

            var trackIndex = config.Persons[personIndex].Tracks.FindIndex(track => track.Id == trackId);
            if (trackIndex < 0)
            {
                throw new Exception($"PersonRepository Track with index {trackId} wasn't found for person with id {personId}.");
            }

            config.Persons[personIndex].Tracks.RemoveAt(trackIndex);

            ConfigHelper.SaveConfig(config);
        }
    }
}
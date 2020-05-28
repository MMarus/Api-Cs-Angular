using System.Collections.Generic;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BooksApi.Services
{
    public class HeroService
    {
        private readonly IMongoCollection<Hero> _heros;

        public HeroService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _heros = database.GetCollection<Hero>(settings.HerosCollectionName);
        }

        public List<Hero> Get() =>
            _heros.Find(hero => true).ToList();

        public Hero Get(string id) =>
            _heros.Find<Hero>(hero => hero.Id == id).FirstOrDefault();

        public Hero Create(Hero hero)
        {
            _heros.InsertOne(hero);
            return hero;
        }

        public void Update(string id, Hero heroIn) =>
            _heros.ReplaceOne(hero => hero.Id == id, heroIn);

        public void Remove(Hero heroIn) =>
            _heros.DeleteOne(hero => hero.Id == heroIn.Id);

        public void Remove(string id) =>
            _heros.DeleteOne(hero => hero.Id == id);

        public ActionResult<List<Hero>> SearchByName(string name) => _heros.Find(hero => hero.Name.ToLower().Contains(name.ToLower())).ToList();
    }
}

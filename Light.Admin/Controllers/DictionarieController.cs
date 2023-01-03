using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Light.Admin.Database;
using Light.Admin.Mongo.Dtos;
using Light.Admin.Mongo.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace Light.Admin.Mongo.Controllers
{
    public class DictionarieController : IDynamicApiController
    {
        private readonly IMongoDbContext db;

        public DictionarieController(
          IMongoDbContext db
            )
        {
            this.db = db;
        }

        /// <summary>
        /// 创建字典项
        /// </summary>
        /// <param name="dto"></param>
        public void Create(DictionarieEditDto dto)
        {
            var dictionarie = dto.Adapt<Dictionarie>();

            db.GetCollection<Dictionarie>(nameof(Dictionarie)).InsertOneAsync(dictionarie);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<Dictionarie>> Find(string name, string key)
        {
            var fb = Builders<Dictionarie>.Filter;
            var result = await db.GetCollection<Dictionarie>(nameof(Dictionarie)).FindAsync(
                fb.Where(p => p.Name.Contains(name)).If(name) &
                fb.Where(p => p.Key == key).If(key));

            return result.ToList();
        }
    }
}
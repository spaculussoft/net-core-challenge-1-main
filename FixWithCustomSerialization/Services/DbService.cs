using FixWithCustomSerialization.Controllers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FixWithCustomSerialization.Services
{
    public class DbService : IDbService
    {
        private readonly IMongoCollection<MediaFile> _mediaFile;

        public DbService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _mediaFile = database.GetCollection<MediaFile>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<MediaFile> GetAsync(string name)
        {
            return await _mediaFile.Find(m => m.Name == name).SingleAsync();
        }

        public async Task CreateAsync(MediaFile model)
        {
            await _mediaFile.InsertOneAsync(model);
        }
    }
}

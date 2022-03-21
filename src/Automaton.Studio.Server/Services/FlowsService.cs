using Automaton.Studio.Server.Config;
using Automaton.Studio.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Automaton.Studio.Server.Services
{
    public class FlowsService
    {
        private readonly IMongoCollection<Flow> flowsCollection;

        public FlowsService(
            IOptions<AutomatonDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            flowsCollection = mongoDatabase.GetCollection<Flow>(
                bookStoreDatabaseSettings.Value.FlowsCollectionName);
        }

        public async Task<IEnumerable<Flow>> GetAsync() =>
            await flowsCollection.Find(_ => true).ToListAsync();

        public async Task<Flow> GetAsync(string id) =>
            await flowsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Flow newBook) =>
            await flowsCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Flow updatedBook) =>
            await flowsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await flowsCollection.DeleteOneAsync(x => x.Id == id);
    }
}

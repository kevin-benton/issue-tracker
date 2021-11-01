using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;

using IssueTracker.Core.Settings;
using Microsoft.Azure.Cosmos.Linq;

namespace IssueTracker.Core.Domain.Issue.ReadModel
{
    public interface IIssueRepository
    {
        Task<Issue> GetAsync(string id, CancellationToken cancellationToken = default);
        Task CreateAsync(Issue issue, CancellationToken cancellationToken = default);
        Task UpdateAsync(Issue issue, CancellationToken cancellationToken = default);
        Task<int> CountAllAsync(CancellationToken cancellationToken = default);
    }

    public class IssueRepository : IIssueRepository
    {
        private const string IssuesCollection = "issues-col";

        private readonly Container _container;

        public IssueRepository(CosmosClient client, ICosmosConfig config)
        {
            _container = client.GetContainer(config.DatabaseName, IssuesCollection);
        }

        public async Task<Issue> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _container.ReadItemAsync<Issue>(id, new PartitionKey(id), null,
                    cancellationToken);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new DocumentNotFoundException($"Document with ID {id} not found.", ex);
            }
        }

        public async Task CreateAsync(Issue issue, CancellationToken cancellationToken = default)
        {
            try
            {
                await _container.CreateItemAsync(issue,
                    new PartitionKey(issue.Id),
                    null,
                    cancellationToken);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                throw new DocumentConflictException($"Item with ID {issue.Id} already exists.", ex);
            }
        }

        public async Task UpdateAsync(Issue issue, CancellationToken cancellationToken)
        {
            await _container.ReplaceItemAsync(issue, issue.Id,
                new PartitionKey(issue.Id),
                null,
                cancellationToken);
        }

        public async Task<int> CountAllAsync(CancellationToken cancellationToken)
        {
            return await _container.GetItemLinqQueryable<Issue>(true)
                .CountAsync(cancellationToken);
        }
    }
}

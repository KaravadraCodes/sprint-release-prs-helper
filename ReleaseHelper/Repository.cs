using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Octokit.Internal;

namespace ReleaseHelper
{
    public class Repository
    {
        private const string Owner = "";
        private const string Token = "";

        private GitHubClient _client;
        private readonly string _repo;

        public Repository(string repository)
        {
            _repo = repository;
            InitializeClient();
        }

        private void InitializeClient()
        {
            var credentials = new InMemoryCredentialStore(new Credentials(Token));
            _client = new GitHubClient(new ProductHeaderValue(_repo), credentials);
        }

        public async Task<Branch> GetBranch(string branchName)
        {
            return (await _client.Repository.Branch.GetAll(Owner, _repo)).FirstOrDefault(b => b.Name == branchName);
        }

        public async Task<Reference> CreateBranch(string branchName, string fromBranch)
        {
            var master = await _client.Git.Reference.Get(Owner, _repo, "heads/" + fromBranch);
            return await _client.Git.Reference.Create(Owner, _repo,
                new NewReference("refs/heads/" + branchName, master.Object.Sha));
        }

        public async Task<CompareResult> GetFileChanges(string fromBranch, string toBranch)
        {
            return await _client.Repository.Commit.Compare(Owner, _repo, toBranch, fromBranch);
        }

        public async Task<PullRequest> CreatePullRequest(string fromBranch, string toBranch, string prBody,
            string prTitle)
        {
            return await _client.PullRequest.Create(Owner, _repo,
                new NewPullRequest(prTitle, fromBranch, toBranch) {Body = prBody, Draft = false});
        }
    }
}
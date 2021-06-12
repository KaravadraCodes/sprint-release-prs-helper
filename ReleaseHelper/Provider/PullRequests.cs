using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using ReleaseHelper.Provider.Data;
using Team = ReleaseHelper.Provider.Data.Team;

namespace ReleaseHelper.Provider
{
    public class PullRequests : IPullRequests
    {
        private const string ProductionBranch = "production";
        private const string StagingBranch = "staging";

        private readonly string _releaseBranch;
        private readonly string _prTitle;
        private readonly string _prBody;
        private readonly List<string> _repositories;

        public PullRequests(Team forTeam, string releaseBranch, string prTitle, string prBody)
        {
            _prBody = prBody;
            _releaseBranch = releaseBranch;
            _prTitle = prTitle;
            _repositories = Repositories.Instance[forTeam];
        }

        public async Task<Dictionary<string, string>> ReleaseAsync()
        {
            var result = new Dictionary<string, string>();

            foreach (var repositoryName in _repositories)
            {
                try
                {
                    var repository = new Repository(repositoryName);
                    var compareResult = await repository.GetFileChanges(StagingBranch, ProductionBranch);
                    if (compareResult.Files.Count > 0)
                    {
                        await CreateReleaseBranch(repository);
                        var pr
                            = await repository.CreatePullRequest(StagingBranch, _releaseBranch, _prBody, _prTitle);
                        result.Add(repositoryName, pr.HtmlUrl);
                    }
                    else
                    {
                        result.Add(repositoryName, "No file changes.");
                    }
                }
                catch (Exception e)
                {
                    result.Add(repositoryName,
                        $"Error occurred: {e.Message}. \n Inner exception: {e.InnerException?.Message}");
                }
            }

            return result;
        }

        public async Task<Dictionary<string, string>> ProductionAsync()
        {
            var result = new Dictionary<string, string>();

            foreach (var repositoryName in _repositories)
            {
                try
                {
                    var repository = new Repository(repositoryName);
                    if (await repository.GetBranch(_releaseBranch) != null)
                    {
                        var compareResult = await repository.GetFileChanges(_releaseBranch, ProductionBranch);
                        if (compareResult.Files.Count > 0)
                        {
                            var pr
                                = await repository.CreatePullRequest
                                    (_releaseBranch, ProductionBranch, _prBody, _prTitle);
                            result.Add(repositoryName, pr.HtmlUrl);
                        }
                        else
                        {
                            result.Add(repositoryName, "No file changes.");
                        }
                    }
                    else
                    {
                        result.Add(repositoryName, "Release is not created for this repository.");
                    }
                }
                catch (Exception e)
                {
                    result.Add(repositoryName,
                        $"Error occurred: {e.Message}. \n Inner exception: {e.InnerException?.Message}");
                }
            }

            return result;
        }

        private async Task CreateReleaseBranch(Repository repository)
        {
            if (await repository.GetBranch(_releaseBranch) == null)
            {
                await repository.CreateBranch(_releaseBranch, ProductionBranch);
            }
        }
    }
}
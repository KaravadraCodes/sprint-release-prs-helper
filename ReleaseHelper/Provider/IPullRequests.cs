using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace ReleaseHelper.Provider
{
    public interface IPullRequests
    {
        Task<Dictionary<string, string>> ReleaseAsync();

        Task<Dictionary<string, string>> ProductionAsync();
    }
}
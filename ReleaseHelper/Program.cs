using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReleaseHelper.Provider;
using Team = ReleaseHelper.Provider.Data.Team;

namespace ReleaseHelper
{
    internal static class Program
    {
        private const string ReleaseBranch = "release/sprint-may-14-2021";
        private const string PrTitle = "Sprint May 14, 2021 Release";
        private const string PrBody = "@KaravadraCodes Please review.";

        private static async Task Main(string[] args)
        {
            // For Release PRs
            var result = Execute(async pulls => await pulls.ReleaseAsync(), FormatResult);

            // For Production PRs
            // var result = Execute(async pulls => await pulls.ProductionAsync(), FormatResult);

            Console.WriteLine(result);

            Console.ReadKey();
        }

        private static string Execute(Func<IPullRequests, Task<Dictionary<string, string>>> pullRequestsFunc,
            Func<Dictionary<string, string>, string> converterFunc)
        {
            var pullRequests =
                pullRequestsFunc.Invoke(new PullRequests(Team.Team1, ReleaseBranch, PrTitle, PrBody));

            return converterFunc.Invoke(pullRequests.Result);
        }

        private static string FormatResult(Dictionary<string, string> prs)
        {
            var stringBuilder = new StringBuilder();

            foreach (var pr in prs)
            {
                stringBuilder.AppendLine($"Service: {pr.Key}, PR: {pr.Value}");
            }

            return stringBuilder.ToString();
        }
    }
}
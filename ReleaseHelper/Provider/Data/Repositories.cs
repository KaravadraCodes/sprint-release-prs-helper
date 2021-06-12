using System;
using System.Collections.Generic;

namespace ReleaseHelper.Provider.Data
{
    public class Repositories
    {
        private static readonly Lazy<Repositories> _instance = new Lazy<Repositories>(() => new Repositories());
        public static Repositories Instance => _instance.Value;

        private readonly Dictionary<Team, List<string>> _repos;

        public Repositories()
        {
            _repos = new Dictionary<Team, List<string>>
            {
                {
                    Team.Team1, new List<string>
                    {
                        "vehicle-service",
                        "location-service",
                        "email-helper-service"
                    }
                },
                {
                    Team.Team2, new List<string>
                    {
                        "environment-service",
                        "nlp-service"
                    }
                }
            };
        }

        public List<string> this[Team team] => _repos[team];
    }
}
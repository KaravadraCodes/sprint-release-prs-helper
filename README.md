# sprint-release-prs-helper
Agile methodology is great, but it may lead developers to create tons of pull requests on each release. This repository contains the integration of Github APIs and automates the release process.

This is an initial setup of the application covering the basic functionality of automated Pull Requests.

**Features**:

- **Release PRs**:
1. Iterates over all the repositories of the team.
2. Checks production <- staging file changes.
3. If file changes are there then a branch would be crated with provided name and a release PR would be created (release-branch <- staging) with provided title and description. We could add a mention in description.

- **Production PRs**:
1. Iterates over all the repositories of the team.
2. Checks if release branch is present with provided name and checks production <- release-branch file changes.
3. If file changes are there then a Production PR would be created (production <- release-branch) with provided title and description. We could add a mention in description here too.

**Few things to take care**,
1. Add all the repositories in Repositories class with your Team's name as a key from Team enum.
2. Add Github Repositories Owner name in the Repository class.
3. Create a personal access token from Github and place it in Repository class.
4. Provide your team's enum member in PullRequests class constructor's first argument at Program file.

Once it is setup in machine with above steps, we will just need to replace the release branch name and title on every launch and utility will do the rest. :tada:


Currently it is a Console application, looking forward to add the better user interface for ease of use.

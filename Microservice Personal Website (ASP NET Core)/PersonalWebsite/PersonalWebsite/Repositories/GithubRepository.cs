using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PersonalWebsite.Models;
using PersonalWebsite.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PersonalWebsite.Repositories
{
    public interface IGithubRepository
    {
        public Task<List<GithubRepo>> GetGithubRepositories();
    }

    public class GithubRepository : IGithubRepository
    {
        private readonly GithubSettings githubSettings;
        private readonly HttpClient Client;

        public GithubRepository(
            IOptions<GithubSettings> githubSettings)
        {
            this.githubSettings = githubSettings.Value;

            Client = new HttpClient();
            Client.BaseAddress = new Uri(this.githubSettings.BaseUrl);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<GithubRepo>> GetGithubRepositories()
        {
            var response = await Client.GetAsync(githubSettings.RepoEndpoint);
            if(response.IsSuccessStatusCode)
            {
                var repoList = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GithubRepo>>(repoList);
            }
            return null;
        }
    }
}

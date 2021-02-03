using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Settings;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalWebsiteWebApi.Services
{
    public interface IGithubProjectUpdaterService
    {
        Task Update();
    }

    public class GithubProjectUpdaterService : IGithubProjectUpdaterService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IGithubProjectRepository githubProjectRepository;
        private readonly GithubSettings githubSettings;

        public GithubProjectUpdaterService(
            IHttpClientFactory httpClientFactory,
            IGithubProjectRepository githubProjectRepository,
            IOptions<GithubSettings> githubSettings)
        {
            this.httpClientFactory = httpClientFactory;
            this.githubProjectRepository = githubProjectRepository;
            this.githubSettings = githubSettings.Value;
        }

        public async Task Update()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, githubSettings.RepositoriesUrl);
            request.Headers.Add("User-Agent", "request");

            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var reponseObjectContainer = JsonConvert.DeserializeObject<List<GithubProject>>(responseJson);
                await githubProjectRepository.PushProjects(reponseObjectContainer);
            }
        }
    }
}

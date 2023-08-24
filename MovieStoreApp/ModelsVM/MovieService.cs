using MovieStoreApp.Repositories;
using Newtonsoft.Json;
using System.Net.Http;


namespace MovieStoreApp.ModelsVM
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private readonly MovieStoreAppRepositoryEF _movieRepository;
        private const string API_KEY = "4d211759108373f2af45a56063922d02";

        public MovieService(HttpClient httpClient, MovieStoreAppRepositoryEF movieRepository)
        {
            _httpClient = httpClient;
            _movieRepository = movieRepository;
        }
        
    }

    public class MovieDetails
    {
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        public string PosterUrl { get; set; }
    }
}

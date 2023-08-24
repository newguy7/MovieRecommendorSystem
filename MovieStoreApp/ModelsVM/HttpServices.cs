using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MovieStoreApp.Models;
using MovieStoreApp.Repositories;
using Newtonsoft.Json;

namespace MovieStoreApp.ModelsVM
{
    public class HttpServices
    {
        private readonly HttpClient _httpClient;

        public HttpServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpServices()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Log the error or throw a custom exception.
                throw new Exception($"URL {url} returned a 404 Not Found");
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonString);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public async Task<byte[]> GetByteArrayAsync(string url)
        {
            var response = await _httpClient.GetAsync(url); // Fetch the HttpResponseMessage, not byte[] directly

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Log the error or throw a custom exception.
                throw new Exception($"URL {url} returned a 404 Not Found");
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> GetAndSaveMovieImageAsync(string url, Movie movie, MovieStoreAppRepositoryEF repository)
        {
            byte[] imageData = await GetByteArrayAsync(url);

            // Here, you'd pass the movie object and the imageData to your repository method to save.
            bool isSaved = repository.AddMovieAndPicture(movie, imageData);

            if (!isSaved)
            {
                throw new Exception("Failed to save the movie and its image to the database.");
            }

            return imageData;
        }
    }
}

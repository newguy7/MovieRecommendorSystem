using MovieStoreApp.Models;
using MovieStoreApp.ModelsDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace MovieStoreApp.ServiceClient
{
    public class ServiceClient
    {
        HttpClient client = new HttpClient();
        bool init = false;
        string baseurl = "https://image.tmdb.org/t/p/w500";

        public ServiceClient() 
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, SslPolicyErrors) => { return true; };
            client = new HttpClient(httpClientHandler) { };
            init = true;
        }

        public async Task<string> GetMovieImageFromTMDB(int tmdbId)
        {
            string apiKey = "18a05265784562c4df40232cb93f86c6";          

            string imageUrl = $"{baseurl}/{tmdbId}?api_key={apiKey}";

            try
            {                
                
                HttpResponseMessage response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                string imageFilePath = $"movie_{tmdbId}.jpg"; 
                File.WriteAllBytes(imageFilePath, imageBytes);

                return imageFilePath;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving movie image from TMDB: {ex.Message}");
                return null;
            }
        }

        
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await client.GetAsync(url);

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
            var response = await client.GetAsync(url); // Fetch the HttpResponseMessage, not byte[] directly

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
    }
}

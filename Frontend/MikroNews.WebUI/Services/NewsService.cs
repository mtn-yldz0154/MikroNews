using MikroNews.WebUI.Dtos.NewsDtos;
using Newtonsoft.Json;

namespace MikroNews.WebUI.Services
{
    public class NewsService : INewsService
    {

        private readonly HttpClient _httpClient;
        public NewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateNewsAsync(CreateNewsDto createNewsDto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7149/api/News", createNewsDto);

            // İsteğin başarısız olması durumunda hata fırlatma
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error creating news: {response.StatusCode}, {errorContent}");
            }
        }


        public async Task DeleteNewsAsync(int id)
        {
            await _httpClient.DeleteAsync("https://localhost:7149/api/News?id=" + id);
        }

        public async Task<List<ResultNewsDto>> GetAllNewsAsync()
        {
            var responseMessage = await _httpClient.GetAsync("https://localhost:7149/api/News");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultNewsDto>>(jsonData);
            return values;
        }

        public async Task<UpdateNewsDto> GetByIdNewsAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync("https://localhost:7149/api/News/" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateNewsDto>();
            return values;
        }

        public async Task UpdateNewsAsync(UpdateNewsDto updateNewsDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateNewsDto>("https://localhost:7149/api/News", updateNewsDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error creating news: {response.StatusCode}, {errorContent}");
            }
        }
    }
}

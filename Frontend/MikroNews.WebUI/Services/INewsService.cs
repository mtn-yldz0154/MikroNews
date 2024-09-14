using MikroNews.WebUI.Dtos.NewsDtos;

namespace MikroNews.WebUI.Services
{
    public interface INewsService
    {
        Task<List<ResultNewsDto>> GetAllNewsAsync();
        Task CreateNewsAsync(CreateNewsDto createNewsDto);
        Task UpdateNewsAsync(UpdateNewsDto updateNewsDto);
        Task DeleteNewsAsync(int id);
        Task<UpdateNewsDto> GetByIdNewsAsync(int id);
    }
}

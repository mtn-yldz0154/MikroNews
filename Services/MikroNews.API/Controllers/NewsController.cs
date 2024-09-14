using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MikroNews.BusinessLayer.Abstract;
using MikroNews.DtoLayer.NewsDtos;
using MikroNews.EntityLayer;

namespace MikroNews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public IActionResult NewsList()
        {
            var news = _newsService.TGetAll();
            return Ok(news);
        }


        [HttpPost]
        public IActionResult CreateNews(CreateNewsDto createNewsDto)
        {
            News news = new()
            {
                Created_Time = DateTime.Now,
                ImageUrl = createNewsDto.ImageUrl,
                Description = createNewsDto.Description,
                OrderNo = createNewsDto.OrderNo,
                Status = createNewsDto.Status,
                Title = createNewsDto.Title,
                Updated_Time = DateTime.Now,

            };

            _newsService.TInsert(news);
            return Ok("Haber Başarıyla Eklendi !");
        }

        [HttpDelete]
        public IActionResult RemoveNews(int id)
        {
            _newsService.TDelete(id);
            return Ok("Haber Başarıyla Silindi");
        }

        [HttpGet("{id}")]
        public IActionResult GetNewsDetailById(int id)
        {
            var value = _newsService.TGetById(id);
            return Ok(value);
        }

        [HttpPut]
        public IActionResult UpdateNews(UpdateNewsDto updateNewsDto)
        {
            News news = new()
            {
                Id=updateNewsDto.Id,
                ImageUrl = updateNewsDto.ImageUrl,
                Description = updateNewsDto.Description,
                OrderNo = updateNewsDto.OrderNo,
                Title = updateNewsDto.Title,
                Updated_Time = DateTime.Now,
                Status= updateNewsDto.Status   
            };
            _newsService.TUpdate(news);
            return Ok("Haber Başarıyla Güncellendi");
        }
    }
}

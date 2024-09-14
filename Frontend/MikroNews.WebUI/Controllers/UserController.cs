using Microsoft.AspNetCore.Mvc;
using MikroNews.WebUI.Services;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MikroNews.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly INewsService _newsService;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public UserController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.Info("Index işlemi başlatıldı."); // Başlangıç logu

                // Haberleri al ve OrderNo'ya göre artan sırada sırala
                var allNews = (await _newsService.GetAllNewsAsync())
                    .OrderBy(n => n.OrderNo)
                    .ToList();

                // Durumlarına göre filtrele
                var filteredNews = allNews.Where(news => news.Status == true).ToList();

                _logger.Info($"Toplam {filteredNews.Count} haber listelendi."); // Haber sayısı logu
                return View(filteredNews);
            }
            catch (Exception ex)
            {
                // Genel hata logu
                _logger.Error(ex, "Index işlemi sırasında bir hata oluştu.");
                return View("Error"); // Hata sayfasına yönlendirme
            }
        }

        public async Task<IActionResult> GetNewsDetail(int id)
        {
            try
            {
                _logger.Info($"GetNewsDetail işlemi başlatıldı. Haber ID: {id}"); // Başlangıç logu

                var news = await _newsService.GetByIdNewsAsync(id);
                if (news == null)
                {
                    _logger.Warn($"ID {id} olan haber bulunamadı."); // Uyarı logu
                    return NotFound(); // Haber bulunamazsa 404 döndür
                }

                _logger.Info($"Haber başarıyla yüklendi. Haber ID: {id}"); // Başarı logu
                return View(news);
            }
            catch (ArgumentNullException ex)
            {
                // Özelleştirilmiş hata logu: Parametre eksik
                _logger.Error(ex, $"GetNewsDetail işlemi sırasında bir hata oluştu. Haber ID: {id}. Eksik parametreler.");
                return View("Error");
            }
            catch (InvalidOperationException ex)
            {
                // Özelleştirilmiş hata logu: Geçersiz operasyon
                _logger.Error(ex, $"GetNewsDetail işlemi sırasında bir hata oluştu. Haber ID: {id}. Geçersiz operasyon.");
                return View("Error");
            }
            catch (Exception ex)
            {
                // Genel hata logu
                _logger.Error(ex, $"GetNewsDetail işlemi sırasında bir hata oluştu. Haber ID: {id}");
                return View("Error");
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MikroNews.DtoLayer.NewsDtos;
using MikroNews.WebUI.Dtos.NewsDtos;
using MikroNews.WebUI.Idnetity;
using MikroNews.WebUI.Models;
using MikroNews.WebUI.Services;
using NLog;

namespace MikroNews.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly INewsService _newsService;
        private readonly UserManager<User> _userManager;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public AdminController(INewsService newsService, UserManager<User> userManager)
        {
            _newsService = newsService;
            _userManager = userManager;
        }

        public IActionResult AdminLayout()
        {
            _logger.Info("AdminLayout sayfası yüklendi.");
            return View();
        }

        public IActionResult Index()
        {
            _logger.Info("Admin Index sayfası yüklendi.");
            return View();
        }

        public IActionResult AddNews()
        {
            _logger.Info("AddNews sayfası yüklendi.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(Dtos.NewsDtos.CreateNewsDto createNewsDto, IFormFile file)
        {
            try
            {
                _logger.Info("AddNews işlemi başlatıldı.");

                var lastOrderNos = (await _newsService.GetAllNewsAsync())
                    .OrderByDescending(n => n.OrderNo)
                    .Select(n => n.OrderNo)
                    .ToList();

                if (createNewsDto.Title == null || createNewsDto.Description == null)
                {
                    _logger.Warn("AddNews işlemi sırasında alanlar eksik.");
                    return Json(new { success = false, message = "Lütfen alanları doldurun." });
                }

                if (createNewsDto.OrderNo == 0)
                {
                    var orderNo = lastOrderNos.FirstOrDefault();
                    createNewsDto.OrderNo = (orderNo != null) ? orderNo + 1 : 1;
                }
                else
                {
                    if (lastOrderNos.Contains(createNewsDto.OrderNo))
                    {
                        _logger.Warn("AddNews işlemi sırasında sıra numarası zaten mevcut.");
                        return Json(new { success = false, message = "Bu sıra numarası zaten var." });
                    }
                }

                if (file == null)
                {
                    _logger.Warn("AddNews işlemi sırasında dosya yüklenmedi.");
                    return Json(new { success = false, message = "Lütfen bir dosya yükleyin." });
                }

                var allowedExtensions = new[] { ".jpg", ".gif", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.Warn("AddNews işlemi sırasında dosya formatı geçersiz.");
                    return Json(new { success = false, message = "Yalnızca jpg, gif, png formatında dosyalar yükleyebilirsiniz." });
                }

                createNewsDto.ImageUrl = UploadFile.Upload(file, createNewsDto.Title);

                await _newsService.CreateNewsAsync(createNewsDto);
                _logger.Info("Haber başarıyla eklendi.");
                return Json(new { success = true, message = "Haber başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AddNews işlemi sırasında bir hata oluştu.");
                return Json(new { success = false, message = "Haber eklenirken bir hata oluştu: " + ex.Message });
            }
        }

        public async Task<IActionResult> GetNews()
        {
            try
            {
                _logger.Info("GetNews işlemi başlatıldı.");

                var orderedNews = (await _newsService.GetAllNewsAsync())
                    .OrderBy(n => n.OrderNo)
                    .ToList();

                _logger.Info("Haberler başarıyla listelendi.");
                return View(orderedNews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetNews işlemi sırasında bir hata oluştu.");
                return View("Error"); // Hata sayfasına yönlendirme
            }
        }

        public async Task<IActionResult> EditNews(int id)
        {
            try
            {
                _logger.Info($"EditNews işlemi başlatıldı. Haber ID: {id}");

                var news = await _newsService.GetByIdNewsAsync(id);

                if (news == null)
                {
                    _logger.Warn($"ID {id} olan haber bulunamadı.");
                    return NotFound();
                }

                _logger.Info($"Haber başarıyla yüklendi. Haber ID: {id}");
                return View(news);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"EditNews işlemi sırasında bir hata oluştu. Haber ID: {id}");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(Dtos.NewsDtos.UpdateNewsDto updateNewsDto, IFormFile file)
        {
            try
            {
                _logger.Info($"EditNews işlemi başlatıldı. Haber ID: {updateNewsDto.Id}");

                var allNews = await _newsService.GetAllNewsAsync();
                var lastOrderNos = allNews
                    .OrderByDescending(n => n.OrderNo)
                    .Select(n => n.OrderNo)
                    .ToList();

                var currentNews = allNews.FirstOrDefault(n => n.Id == updateNewsDto.Id);

                if (currentNews != null && currentNews.OrderNo != updateNewsDto.OrderNo)
                {
                    if (lastOrderNos.Contains(updateNewsDto.OrderNo))
                    {
                        _logger.Warn("EditNews işlemi sırasında sıra numarası zaten mevcut.");
                        return Json(new { success = false, message = "Bu sıra numarası zaten var." });
                    }
                }

                if (updateNewsDto.Title == null || updateNewsDto.Description == null)
                {
                    _logger.Warn("EditNews işlemi sırasında alanlar eksik.");
                    return Json(new { success = false, message = "Lütfen alanları doldurun." });
                }

                if (file != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".gif", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        _logger.Warn("EditNews işlemi sırasında dosya formatı geçersiz.");
                        return Json(new { success = false, message = "Yalnızca jpg, gif, png formatında dosyalar yükleyebilirsiniz." });
                    }

                    updateNewsDto.ImageUrl = UploadFile.Upload(file, updateNewsDto.Title);
                }
                else
                {
                    var news = await _newsService.GetByIdNewsAsync(updateNewsDto.Id);
                    if (news != null)
                    {
                        if (currentNews.Title != updateNewsDto.Title)
                        {
                            var existingImageUrl = news.ImageUrl;
                            if (!string.IsNullOrEmpty(existingImageUrl))
                            {
                                var oldFileName = Path.GetFileNameWithoutExtension(existingImageUrl);
                                var extension = Path.GetExtension(existingImageUrl);
                                var newFileName = updateNewsDto.Title;
                                var newFileUrl = $"{newFileName}{extension}";

                                var oldFilePath = Path.Combine("wwwroot/CmsFiles", existingImageUrl);
                                var newFilePath = Path.Combine("wwwroot/CmsFiles", newFileUrl);

                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Move(oldFilePath, newFilePath);
                                }

                                updateNewsDto.ImageUrl = newFileUrl;
                            }
                        }
                        else
                        {
                            updateNewsDto.ImageUrl = news.ImageUrl;
                        }
                    }
                }

                await _newsService.UpdateNewsAsync(updateNewsDto);
                _logger.Info($"Haber başarıyla güncellendi. Haber ID: {updateNewsDto.Id}");
                return Json(new { success = true, message = "Haber başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"EditNews işlemi sırasında bir hata oluştu. Haber ID: {updateNewsDto.Id}");
                return Json(new { success = false, message = "Haber güncellenirken bir hata oluştu: " + ex.Message });
            }
        }

        public async Task<IActionResult> DeleteNews(int id)
        {
            try
            {
                _logger.Info($"DeleteNews işlemi başlatıldı. Haber ID: {id}");

                await _newsService.DeleteNewsAsync(id);
                _logger.Info($"Haber başarıyla silindi. Haber ID: {id}");

                TempData["AlertMessage"] = "Haber başarıyla silindi.";
                return RedirectToAction("GetNews");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"DeleteNews işlemi sırasında bir hata oluştu. Haber ID: {id}");
                TempData["AlertMessage"] = "Haber silinirken bir hata oluştu.";
                return RedirectToAction("GetNews");
            }
        }

        public IActionResult GetAdmins()
        {
            _logger.Info("GetAdmins işlemi başlatıldı.");

            var admins = _userManager.Users.ToList();
            _logger.Info($"Toplam {admins.Count} admin listelendi.");

            return View(admins);
        }

        public IActionResult Register()
        {
            _logger.Info("Register sayfası yüklendi.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warn("Register işlemi sırasında model geçersiz.");
                    return View(model);
                }

                var user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.Info("Yeni kullanıcı başarıyla kaydedildi.");
                    return RedirectToAction("GetAdmins", "Admin");
                }

                _logger.Warn("Register işlemi sırasında kullanıcı oluşturulamadı. Hatalar: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Register işlemi sırasında bir hata oluştu.");
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu.");
                return View(model);
            }
        }
    }
}

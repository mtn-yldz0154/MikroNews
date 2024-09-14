namespace MikroNews.WebUI.Services
{
    public class UploadFile
    {
        public static string Upload(IFormFile file,string name)
        {
            var extension = Path.GetExtension(file.FileName);
            var newFileName = name+ extension;
            var location = "";        
             location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CmsFiles/", newFileName);
            var stream = new FileStream(location, FileMode.Create);
            file.CopyTo(stream);
            return newFileName;
        }
    }
}

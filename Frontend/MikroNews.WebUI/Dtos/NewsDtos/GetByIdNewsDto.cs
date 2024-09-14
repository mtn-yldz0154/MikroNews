namespace MikroNews.WebUI.Dtos.NewsDtos
{
    public class GetByIdNewsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Created_Time { get; set; }
        public DateTime Updated_Time { get; set; }

        public int OrderNo { get; set; }

        public bool Status { get; set; }
    }
}

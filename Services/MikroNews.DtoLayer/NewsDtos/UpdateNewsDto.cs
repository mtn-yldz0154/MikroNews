using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroNews.DtoLayer.NewsDtos
{
    public class UpdateNewsDto
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

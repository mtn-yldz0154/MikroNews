using MikroNews.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroNews.DataAccessLayer.Abstract
{
    public interface INewsDalRepository:IGenericDalRepository<News>
    {
    }
}

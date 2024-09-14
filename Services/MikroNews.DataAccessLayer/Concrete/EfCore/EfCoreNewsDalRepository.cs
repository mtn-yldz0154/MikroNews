using MikroNews.DataAccessLayer.Abstract;
using MikroNews.DataAccessLayer.Context;
using MikroNews.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroNews.DataAccessLayer.Concrete.EfCore
{
    public class EfCoreNewsDalRepository:EfCoreGenericDalRepository<News,NewsContext>,INewsDalRepository
    {
    }
}

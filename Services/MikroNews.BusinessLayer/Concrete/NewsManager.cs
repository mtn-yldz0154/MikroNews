using MikroNews.BusinessLayer.Abstract;
using MikroNews.DataAccessLayer.Abstract;
using MikroNews.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroNews.BusinessLayer.Concrete
{
    public class NewsManager:INewsService
    {
        private INewsDalRepository _newsDalRepository;

        public NewsManager(INewsDalRepository newsDalRepository)
        {
            _newsDalRepository = newsDalRepository;
        }

        public void TDelete(int id)
        {
            _newsDalRepository.Delete(id);
        }

        public List<News> TGetAll()
        {
            return _newsDalRepository.GetAll();
        }

        public News TGetById(int id)
        {
            return _newsDalRepository.GetById(id);
        }

        public void TInsert(News entity)
        {
            _newsDalRepository.Insert(entity);
        }

        public void TUpdate(News entity)
        {
            _newsDalRepository.Update(entity);
        }

    }
}

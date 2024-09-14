using Microsoft.EntityFrameworkCore;
using MikroNews.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroNews.DataAccessLayer.Concrete.EfCore
{
    public class EfCoreGenericDalRepository<TEntity, TContext> : IGenericDalRepository<TEntity> where TEntity : class
        where TContext : DbContext, new()
    {
        public void Delete(int id)
        {
            using (var context = new TContext())
            {
                var entity = context.Set<TEntity>().Find(id);
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
        }

        public List<TEntity> GetAll()
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }

        public void Insert(TEntity entity)
        {
            using (var db = new TContext())
            {
                db.Set<TEntity>().Add(entity);
                db.SaveChanges();

            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Update(entity);
                context.SaveChanges();
            }
        }
    }
}

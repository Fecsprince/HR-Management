using System.Collections.Generic;
using System.Data.Entity;
using HRManagement.Data;
using HRManagement.Data.Models;
using HRManagement.Services.Interfaces;

namespace HRManagement.Services.Implementations
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {

        internal DataContext context;
        internal DbSet<T> Dbset;

        public SQLRepository(DataContext dbContext)
        {
            context = dbContext;
            Dbset = context.Set<T>();
        }


        public IEnumerable<T> Collection()
        {
            return Dbset;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public bool Delete(string Id)
        {
            var t = Find(Id);
            if (t != null)
            {
                Dbset.Remove(t);
                Commit();
                return true;
            }
            return false;

        }

        public T Find(string Id)
        {
            return Dbset.Find(Id);
        }

        public T Insert(T t)
        {
            Dbset.Add(t);
            Commit();
            return t;
        }

        public T Update(T t)
        {
            Dbset.Attach(t);
            context.Entry(t).State = EntityState.Modified;
            Commit();
            return t;
        }
    }
}

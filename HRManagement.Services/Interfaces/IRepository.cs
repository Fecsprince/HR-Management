using HRManagement.Data.Models;
using System.Collections.Generic;

namespace HRManagement.Services.Interfaces 
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> Collection();    
        void Commit();
        bool Delete(string Id);
        T Find(string Id);
        T Insert(T t);
        T Update(T t);
    }
}
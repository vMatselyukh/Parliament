using System.Collections.Generic;

namespace WebApi.Dal.Interfaces
{
    public interface IBaseRepository<T>
    {
        T Get(int id);
        IList<T> GetAll();
        T Upsert(T t);
        void Delete(T t);
        void Delete(int id);
    }
}
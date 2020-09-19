using System.Collections.Generic;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IRepository<T> {
        IEnumerable<T> Get(ISpecification<T> specification);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}

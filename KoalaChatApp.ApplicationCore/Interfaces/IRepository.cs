using KoalaChatApp.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IRepository<T> where T : BaseEntity {
        IEnumerable<T> Get(ISpecification<T> specification);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}

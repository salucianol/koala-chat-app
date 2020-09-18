using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KoalaChatApp.ApplicationCore.Entities;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface ISpecification<T> where T : BaseEntity {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public List<string> IncludeStrings { get; set; }
    }
}

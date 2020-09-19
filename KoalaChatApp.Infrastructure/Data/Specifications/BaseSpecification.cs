using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KoalaChatApp.Infrastructure.Data.Specifications {
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity {
        public Expression<Func<T, bool>> Criteria { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Expression<Func<T, object>>> Includes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<string> IncludeStrings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddInclude(Expression<Func<T, object>> includeExpression) {
            Includes.Add(includeExpression);
        }

        public void AddInclude(string includeString) {
            IncludeStrings.Add(includeString);
        }
    }
}

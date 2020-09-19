using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface ISpecification<T> {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; }
        public List<string> IncludeStrings { get; }

        void AddInclude(Expression<Func<T, object>> includeExpression);
        void AddInclude(string includeString);
    }
}

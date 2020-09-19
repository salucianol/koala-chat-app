using KoalaChatApp.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace KoalaChatApp.ApplicationCore.Specifications {
    public abstract class BaseSpecification<T> : ISpecification<T> {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        public BaseSpecification(Expression<Func<T, bool>> criteria) {
            this.Criteria = criteria;
        }

        void ISpecification<T>.AddInclude(Expression<Func<T, object>> includeExpression) {
            Includes.Add(includeExpression);
        }

        void ISpecification<T>.AddInclude(string includeString) {
            IncludeStrings.Add(includeString);
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using LinFu.Finders;
using LinFu.Finders.Interfaces;

namespace NotQuiteLisp.Visitors
{
    internal class VisitableItem<TResult>
    {
        private readonly object _targetItem;
        public VisitableItem(object targetItem)
        {
            if (targetItem == null)
                throw new ArgumentNullException("targetItem");

            _targetItem = targetItem;
        }

        public TResult Accept(object visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            var visitorType = visitor.GetType();
            var visitorMethods = visitorType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name.StartsWith("Visit"))
                .AsFuzzyList();

            visitorMethods.AddCriteria(m => m.ReturnType == typeof(TResult), CriteriaType.Critical);
            visitorMethods.AddCriteria(m => m.GetParameters().Count() == 1, CriteriaType.Critical);

            // Force covariant parameter types
            visitorMethods.AddCriteria(m =>
            {
                var parameters = m.GetParameters().ToArray();
                var firstParameterType = parameters.Length > 0 ? parameters.First().ParameterType : null;
                return firstParameterType != null && _targetItem.GetType().IsAssignableFrom(firstParameterType);
            });

            // Optionally match the exact parameter type
            visitorMethods.AddCriteria(
                m =>
                {
                    var parameters = m.GetParameters().ToArray();
                    var firstParameterType = parameters.Length > 0 ? parameters.First().ParameterType : null;
                    return firstParameterType != null && _targetItem.GetType() == firstParameterType;
                },
                CriteriaType.Optional);

            var bestMatch = visitorMethods.BestMatches().FirstOrDefault();
            if (bestMatch == null)
                throw new VisitorMethodNotFoundException();

            var targetMethod = bestMatch.Item;

            var result = targetMethod.Invoke(visitor, new object[] { _targetItem });

            return (TResult)result;
        }
    }
}
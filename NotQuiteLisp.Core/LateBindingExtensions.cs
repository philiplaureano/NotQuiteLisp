namespace NotQuiteLisp.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    using LinFu.Finders;
    using LinFu.Finders.Interfaces;

    public static class LateBindingExtensions
    {
        public static object Invoke(this object target, string methodName, params object[] args)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var targetType = target.GetType();
            var methodList = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToArray().AsFuzzyList();

            // Match the method name
            methodList.AddCriteria(m => m.Name == methodName, CriteriaType.Critical);

            var argList = args ?? new object[0];
            var parameterCount = argList.Length;

            for (var i = 0; i < parameterCount; i++)
            {
                var currentArgument = argList[i];
                if (currentArgument == null)
                    continue;

                var argumentType = argList[i].GetType();
                var currentIndex = i;

                Func<MethodInfo, bool> hasCompatibleParameterType = m =>
                    {
                        var parameters = m.GetParameters().ToArray();
                        if (currentIndex >= parameters.Length)
                            return false;

                        var parameterType = parameters[currentIndex].ParameterType;
                        return parameterType.IsAssignableFrom(argumentType);
                    };

                Func<MethodInfo, bool> hasExactParameterType = m =>
                    {
                        var parameters = m.GetParameters().ToArray();
                        if (currentIndex >= parameters.Length)
                            return false;

                        var parameterType = parameters[currentIndex].ParameterType;
                        return parameterType == argumentType;
                    };

                methodList.AddCriteria(hasCompatibleParameterType);
                methodList.AddCriteria(hasExactParameterType, CriteriaType.Optional);
            }

            var bestMatch = methodList.BestMatches().FirstOrDefault();
            if (bestMatch == null || bestMatch.Item == null)
                throw new MethodNotFoundException(methodName);

            var targetMethod = bestMatch.Item;
            var targetParameterCount = targetMethod.GetParameters().Count();

            var trimmedArgs = (args ?? new object[0]).ToArray();
            if (targetParameterCount > 0)
                trimmedArgs = trimmedArgs.Take(targetParameterCount).ToArray();

            return targetMethod.Invoke(target, trimmedArgs);
        }
    }
}
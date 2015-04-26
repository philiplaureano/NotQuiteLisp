namespace NotQuiteLisp.Core
{
    using System;

    public class MethodNotFoundException : Exception
    {
        public MethodNotFoundException(string methodName)
            : base(string.Format("No compatible method found for method named '{0}'", methodName))
        {
        }
    }
}
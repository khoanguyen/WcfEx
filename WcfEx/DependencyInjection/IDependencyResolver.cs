using System;

namespace WcfEx.DependencyInjection
{
    /// <summary>
    /// Dependency Resolver solves dependencies when creating an object 
    /// or injecting the dependencies into existing object
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Create an instance of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructorParams"></param>
        /// <returns></returns>
        T CreateInstance<T>(object constructorParams = null);

        /// <summary>
        /// Create an instance of given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constructorParams"></param>
        /// <returns></returns>
        object CreateInstance(Type type, object constructorParams = null);

        /// <summary>
        /// Inject dependencies into an existing object
        /// </summary>
        /// <param name="target"></param>
        void Inject(object target);
    }
}

namespace WcfEx.DependencyInjection
{
    /// <summary>
    /// Factory for creating Dependency Resolver
    /// </summary>
    public interface IDependencyResolverFactory        
    {
        IDependencyResolver CreateResolver();
    }
}

using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using WcfExt.DependencyInjection;

namespace WcfExt.Behaviors
{
    public class DiServiceBehavior : ServiceBehaviorBase
    {
        private readonly IDependencyResolverFactory _factory;
        public Type DependencyResolverFactoryType { get; private set; }        

        public DiServiceBehavior(Type dependencyResolverFactoryType)
        {
            DependencyResolverFactoryType = dependencyResolverFactoryType;
            _factory = Activator.CreateInstance(DependencyResolverFactoryType) as IDependencyResolverFactory;
        }

        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            base.ApplyDispatchBehavior(serviceDescription, serviceHostBase);            
            
            // Apply DI InstanceContext initializer on endpoints
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
            {
                foreach (var epDispatcher in dispatcher.Endpoints)
                {
                    // Create resolver
                    var resolver = _factory.CreateResolver();
                    var diInstanceContextInitializer = new DiInstanceContextInitializer(resolver);
                    epDispatcher.DispatchRuntime
                                .InstanceContextInitializers
                                .Add(diInstanceContextInitializer);
                }
            }
        }
    }
}

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WcfExt.DependencyInjection
{
    /// <summary>
    /// DI InstanceContext initializer for calling the DependencyResolver
    /// at runtime to inject dependencies into Service instance
    /// </summary>
    public class DiInstanceContextInitializer : IInstanceContextInitializer
    {
        /// <summary>
        /// Dependency resolver
        /// </summary>
        public IDependencyResolver Resolver { get; private set; }

        public DiInstanceContextInitializer(IDependencyResolver resolver)
        {
            Resolver = resolver;
        }

        /// <summary>
        /// Initialize InstanceContext
        /// </summary>
        /// <param name="instanceContext"></param>
        /// <param name="message"></param>
        public void Initialize(InstanceContext instanceContext, Message message)
        {
            // Ensure handler's removal
            instanceContext.Opened -= InstanceContextOpendedHandler;

            // Add event handler for Opened event
            instanceContext.Opened += InstanceContextOpendedHandler;
        }

        /// <summary>
        /// Handler for injecting dependencies when Service instance is ready to be used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstanceContextOpendedHandler(object sender, EventArgs e)
        {
            if (!(sender is InstanceContext)) return;

            // Inject dependencies on service instance
            var context = sender as InstanceContext;
            var serviceInstance = context.GetServiceInstance();
            Resolver.Inject(serviceInstance);
        }
    }
}

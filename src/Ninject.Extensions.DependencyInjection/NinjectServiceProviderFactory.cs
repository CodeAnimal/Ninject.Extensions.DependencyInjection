using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ninject.Extensions.DependencyInjection
{
	public class NinjectServiceProviderFactory : IServiceProviderFactory<IKernel>
	{
		private readonly Action<IKernel> configurationAction;
        private readonly INinjectSettings ninjectSettings;


		public NinjectServiceProviderFactory(Action<IKernel> configurationAction = null)
            : this(null, configurationAction)
        {
        }

		public NinjectServiceProviderFactory(INinjectSettings ninjectSettings, Action<IKernel> configurationAction = null)
        {
            this.ninjectSettings = ninjectSettings;
            this.configurationAction = configurationAction ?? (_ => { });
        }

		public IKernel CreateBuilder(IServiceCollection services)
        {
            var kernel = new NetCoreKernel(ninjectSettings ?? new NinjectSettings());
            
			var adapter = new ServiceCollectionAdapter();
			adapter.Populate(kernel, services);
			
			configurationAction(kernel);
			
			return kernel;
		}

        public IServiceProvider CreateServiceProvider(IKernel containerBuilder)
		{
			if (containerBuilder == null) throw new ArgumentNullException(nameof(containerBuilder));
			
			var builder = new NinjectServiceProviderBuilder((NetCoreKernel) containerBuilder);
			
			return builder.Build();
		}
    }
}

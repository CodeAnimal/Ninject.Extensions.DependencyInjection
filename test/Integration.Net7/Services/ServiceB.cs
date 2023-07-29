using Integration.Net7.Services.Abstractions;

namespace Integration.Net7.Services
{
    internal class ServiceB : IServiceB
    {
        private readonly IServiceA serviceA;

        public ServiceB(IServiceA serviceA)
        {
            this.serviceA = serviceA;
        }

        public string GetFromSubService(int size)
        {
            return serviceA.GetRandomString(size);
        }
    }
}
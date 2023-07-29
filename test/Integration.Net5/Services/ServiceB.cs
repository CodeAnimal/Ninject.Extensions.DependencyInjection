using Integration.Net5.Services.Abstractions;

namespace Integration.Net5.Services
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
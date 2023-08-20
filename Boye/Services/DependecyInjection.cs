using Boye.Repository;

namespace Boye.Services
{
    public class DependecyInjection
    {
        public static void Register(IServiceCollection services)
        {
            services

                .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<LoginValidator>()
                .AddTransient<BoyeRepository>();
                //.AddTransient<StoreContext>();
        }
    }
}

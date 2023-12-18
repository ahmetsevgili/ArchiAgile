using ArchiAgile.Server.Data;
using ArchiAgile.Server.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ArchiAgile.Server.Services
{
    public class CacheService: ICacheService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;

        private const string _userImagePath = "USER_IMAGE_PATH";
        public CacheService(IMemoryCache memoryCache,
                    IServiceProvider serviceProvider
    )
        {
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
        }
        private T CallSetMethod<T>(Func<ApplicationDBContext, T> setFunc)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                return setFunc(context);
            }
        }

        private void CallAllSetMethod(Action<ApplicationDBContext> setAction)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                setAction(context);
            }
        }
        public void Load()
        {
            Reload();
        }
        public void Reload()
        {
            CallAllSetMethod((ApplicationDBContext context) =>
            {
                SetUserImagePath(context);
            });
        }

        public string GetUserImagePath()
        {
            if (!_memoryCache.TryGetValue(_userImagePath, out string val))
            {
                val = CallSetMethod(SetUserImagePath);
            }

            return val;
        }

        public string SetUserImagePath(ApplicationDBContext dbContext)
        {
            var param = dbContext.Parameter.FirstOrDefault(q => q.Name == _userImagePath);

            if (param == null)
            {
                return "";
            }

            if (string.IsNullOrWhiteSpace(param.Value))
            {
                return "";
            }

            var paramVal = param.Value;
            _memoryCache.Set(_userImagePath, paramVal);

            return paramVal;
        }
    }
}

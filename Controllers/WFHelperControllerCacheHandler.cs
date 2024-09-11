using System.Threading.Tasks;

namespace Voith.WF.OrderReleaseVP.Business.Controllers;

internal sealed class WFHelperControllerCacheHandler<TKey, TValue>
{
    private object _lock;

    public WFHelperControllerCacheHandler()
    {
        _lock = new object();
    }

    public delegate bool TryGetValueDelegate(TKey key, out Task<TValue> value);
    public delegate void AddDelegate(TKey key, Task<TValue> value);
    public delegate void UpdateDelegate(TKey key, Task<TValue> value);
    public delegate Task<TValue> AcquireDelegate();

    public Task<TValue> Get(
            TKey key,
            TryGetValueDelegate tryGetValue,
            AddDelegate add,
            AcquireDelegate acquire,
            UpdateDelegate update,
            int refreshAfter)
    {
        Task<TValue> AcquireAndSchedule()
        {
            Task<TValue> ret = acquire();
            
            _ = Task.Delay(refreshAfter).ContinueWith((Task _) => {
                AcquireAndSchedule().ContinueWith((completed) => {
                    lock(_lock)
                        update(key, completed);
                });
            });

            return ret;
        }

        Task<TValue> ret;
        lock(_lock)
        {
            if(! tryGetValue(key, out ret!))
            {
                add(
                    key,
                    ret = AcquireAndSchedule()
                );
            }
        }
        return ret;
    }
}
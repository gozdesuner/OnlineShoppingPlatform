using Microsoft.Extensions.Caching.Memory;

namespace OnlineShoppingPlatform.BL.Helpers
{
    public class MaintenanceState
    {
        private const string MaintenanceKey = "MaintenanceMode";
        private readonly IMemoryCache _memoryCache;

        public MaintenanceState(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Bakım modu aktif mi?
        public bool IsInMaintenance()
        {
            return _memoryCache.TryGetValue(MaintenanceKey, out bool isMaintenance) && isMaintenance;
        }

        // Bakım modunu aktif veya pasif yap
        public void SetMaintenanceMode(bool isMaintenance)
        {
            _memoryCache.Set(MaintenanceKey, isMaintenance);
        }
    }
}

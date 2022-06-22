using BattleshipStateTracker.Core.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace BattleshipStateTracker.Core.Services.Implementations
{
    public class MemoryCacheWrapper : IMemoryCacheWrapper
    {
        #region Local variables

        private readonly ILogger<MemoryCacheWrapper> _logger;
        private readonly IMemoryCache _memoryCache;

        #endregion Local variables

        #region Constructor

        public MemoryCacheWrapper(
            ILogger<MemoryCacheWrapper> logger,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        #endregion Constructor

        #region Public methods

        /// <inheritdoc />
        public (bool, T) GetCache<T>(string key) where T : class
        {
            try
            {
                var isAvailable = _memoryCache.TryGetValue(key, out T cachedObject);
                return (isAvailable, cachedObject);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetCache<T>(string key)");
                return (false, default);
            }
        }

        /// <inheritdoc />
        public void SetCache<T>(string key, T data) where T : class
        {
            try
            {
                // Set cache expiration to 2 minutes
                _memoryCache.Set(key, data, TimeSpan.FromDays(1));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SetCache<T>(string key, T data)");
            }
        }

        #endregion Public methods
    }
}
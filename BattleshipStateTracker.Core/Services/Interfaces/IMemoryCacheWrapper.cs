namespace BattleshipStateTracker.Core.Services.Interfaces
{
    public interface IMemoryCacheWrapper
    {
        /// <summary>
        /// Get cache data if available
        /// </summary>
        (bool, T) GetCache<T>(string key) where T : class;

        /// <summary>
        /// Get cache data if available
        /// </summary>
        void SetCache<T>(string key, T data) where T : class;
    }
}
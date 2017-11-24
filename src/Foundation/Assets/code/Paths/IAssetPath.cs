using System.Collections.Generic;

namespace Lotus.Foundation.Assets.Paths
{
    public interface IAssetPath
    {
        string GetKey();
        IEnumerable<string> GetTargets();
        int GetCacheExpiryHours();

        void ProcessRequest(AssetRequest request);
        bool ProcessFile(AssetRequest request);
        void ProcessTimestamp(AssetRequest request);
    }
}

using System.Collections.Generic;
using System.Web;

namespace Lotus.Foundation.Assets.Paths
{
    public interface IAssetPath
    {
        string GetKey();
        IEnumerable<string> GetTargets();
        int GetCacheExpiryHours();
        
        void ProcessRequest(HttpContext context, string relativePath, string extension, int timestamp);
        void ProcessTimestamp(HttpContext context, string relativePath, string extension, int timestamp);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lotus.Foundation.Assets.Paths.Extension;
using Lotus.Foundation.Assets.Paths.Results;

namespace Lotus.Foundation.Assets.Paths
{
    public interface IAssetPath
    {
        string GetKey();
        int GetExpireCache();
        IEnumerable<string> GetTargets();
        IEnumerable<string> GetFileNames();
        IEnumerable<string> GetIgnore();
        
        ExtensionResult ProcessRequest(HttpContext context);
    }
}

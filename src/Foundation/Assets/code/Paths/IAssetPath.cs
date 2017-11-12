using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lotus.Foundation.Assets.Paths
{
    public interface IAssetPath
    {
        string GetKey();
        void Handle(HttpContext context);
    }
}

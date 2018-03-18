using Sitecore.Data;
using Sitecore;

namespace Lotus.Foundation.Kernel.Utils.SitecoreUtils
{
    public static class MainUtil
    {
        [CanBeNull]
        public static ID TryParseIdOrNull(string idStr)
        {
            ID id;
            return ID.TryParse(idStr, out id) ? id : null;
        }
        
        [CanBeNull]
        public static ShortID TryParseShortIdOrNull(string idStr)
        {
            ShortID shortid;
            return ShortID.TryParse(idStr, out shortid) ? shortid : null;
        }
    }
}
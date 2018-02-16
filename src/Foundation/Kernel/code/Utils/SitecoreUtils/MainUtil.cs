using Sitecore.Data;

namespace Lotus.Foundation.Kernel.Utils.SitecoreUtils
{
    public static class MainUtil
    {
        public static ID TryParseIdOrNull(string idStr)
        {
            ID id;
            return ID.TryParse(idStr, out id) ? id : null;
        }
        
        public static ShortID TryParseShortIdOrNull(string idStr)
        {
            ShortID shortid;
            return ShortID.TryParse(idStr, out shortid) ? shortid : null;
        }
    }
}
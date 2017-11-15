using System.Collections.Generic;
using System.Web;
using Lotus.Foundation.Assets.Paths.Results;

namespace Lotus.Foundation.Assets.Paths.Extension
{
    public class ExtensionPath : IAssetPath
    {
        public string Key { get; set; }
        public string Targets { get; set; }
        public string FileNames { get; set; }
        public string Ignore { get; set; }

        public int _expireHours;
        public string ExpireCache
        {
            get { return _expireHours.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var time = int.Parse(value.Substring(0, value.Length - 1));
                    var format = value.Substring(value.Length - 1);
                    switch (format.ToLower())
                    {
                        default:
                            _expireHours = time;
                            break;
                           
                        case "d":
                            _expireHours = 24 * time;
                            break;
                        
                        case "m":
                            _expireHours = (30 * 24) * time;
                            break;
                    }
                }
                else
                {
                    _expireHours = 0;
                }
            }
        }
        
        public virtual string GetKey()
        {
            return Key;
        }

        public virtual int GetExpireCache()
        {
            return _expireHours;
        }

        public virtual IEnumerable<string> GetTargets()
        {
            return Targets.Split('|');
        }
        
        public virtual IEnumerable<string> GetFileNames()
        {
            return FileNames.Split('|');
        }

        public virtual IEnumerable<string> GetIgnore()
        {
            return Ignore.Split('|');
        }
        
        public virtual ExtensionResult ProcessRequest(HttpContext context)
        {
            return new JSResult();
        }
    }
}
using System.Collections.Generic;
using System.Web;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Assets.Pipelines;
using Lotus.Foundation.Extensions.Regex;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets.Paths
{
    public class BasePath : IAssetPath
    {
        public virtual string Key { get; set; }
        public virtual string Targets { get; set; }
        
        public int _expireHours;
        public virtual string ExpireCache
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

        public virtual IEnumerable<string> GetTargets()
        {
            return Targets.Split(':');
        }

        public virtual int GetCacheExpiryHours()
        {
            return _expireHours;
        }
        
        public virtual void ProcessRequest(HttpContext context, string relativePath, string extension, int timestamp)
        {
            ProcessTimestamp(context, relativePath, extension, timestamp);

            var pipelineArgs = new AssetPipelineArgs(context, this, relativePath, extension, timestamp);
            foreach (var pipeline in Global.Pipelines)
            {
                pipeline.Process(pipelineArgs);
            }

            if (!context.WriteFile(relativePath))
            {
                context.NotFound();
            }
        }

        public virtual void ProcessTimestamp(HttpContext context, string relativePath, string extension, int timestamp)
        {
            var modified = AssetsRequestHelper.ExtractTimestampFromFile(context, relativePath);

            if (timestamp <= 0)
            {
                context.RedirectIgnored("~/" + relativePath);
            }
            
            if (timestamp != modified)
            {
                context.RedirectWithUpdate(modified, relativePath, extension);
            }

            var origional = context.Request.RawUrl;
            
            if (!string.IsNullOrEmpty(origional) && !origional.IsMatch("^/-/assets/"))
            {
                context.RedirectWithUpdate(modified, relativePath, extension);
            }
        }

        public virtual bool CheckTarget(string target, string pattern)
        {
            return !string.IsNullOrEmpty(pattern) && target.IsMatch(pattern);
        }
    }
}
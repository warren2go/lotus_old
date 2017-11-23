using System.Collections.Generic;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Extensions.RegularExpression;
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
        
        public virtual void ProcessRequest(AssetRequest request)
        {
            ProcessTimestamp(request);

            foreach (var pipeline in Global.Pipelines)
            {
                pipeline.Process(request);
            }

            if (!request.Context.WriteFile(request.RelativePath))
            {
                if (!string.IsNullOrEmpty(AssetsSettings.NotFoundUrl))
                {
                    request.Context.Redirect(AssetsSettings.NotFoundUrl);
                }
                request.Context.NotFound();
            }
        }

        public virtual void ProcessTimestamp(AssetRequest request)
        {
            if (!AssetsRequestHelper.FileExists(request.Context, request.RelativePath))
            {
                request.Context.RedirectIgnored("~/" + request.RelativePath);
            }
            
            var modified = AssetsRequestHelper.ExtractTimestampFromFile(request.Context, request.RelativePath);

            if (request.Timestamp != modified)
            {
                request.Context.RedirectWithUpdate(modified, request.RelativePath, request.Extension);
            }

            var origional = request.Context.Request.RawUrl;
            
            if (!string.IsNullOrEmpty(origional) && !origional.IsMatch("^/-/assets/"))
            {
                request.Context.RedirectWithUpdate(modified, request.RelativePath, request.Extension);
            }
        }

        public virtual bool CheckTarget(string target, string pattern)
        {
            return !string.IsNullOrEmpty(pattern) && target.IsMatch(pattern);
        }
    }
}
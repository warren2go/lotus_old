using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Regex;

namespace Lotus.Foundation.Assets.Paths.Extension
{
    public class ExtensionPath : BasePath
    {
        public string FileNames { get; set; }
        public string Ignore { get; set; }

        private IEnumerable<string> GetFileNames()
        {
            return FileNames.Split('|');
        }
        
        private IEnumerable<string> GetIgnore()
        {
            return Ignore.Split('|');
        }
        
        public override void ProcessRequest(HttpContext context, string relativePath, string extension, int timestamp)
        {
            var fileName = relativePath.ExtractPattern(AssetsSettings.Regex.FileName);
            var fileNames = GetFileNames();
            foreach (var allowed in fileNames)
            {
                if (!string.IsNullOrEmpty(allowed) && !fileName.IsMatch(allowed))
                {
                    context.RedirectBad("~/" + relativePath);
                }
            }
            var ignore = GetIgnore();
            foreach (var ignored in ignore)
            {
                if (!string.IsNullOrEmpty(ignored) && fileName.IsMatch(ignored))
                {
                    context.RedirectIgnored("~/" + relativePath);
                }
            }
            if (!GetTargets().Any(x => CheckTarget(extension, x)))
            {
                context.RedirectBad("~/" + relativePath);
            }
            base.ProcessRequest(context, relativePath, extension, timestamp);
        }
    }
}
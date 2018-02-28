using System.Collections.Generic;
using System.Linq;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;

namespace Lotus.Foundation.Assets.Paths.Folder
{
    public class FolderPath : BasePath
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
        
        public override void ProcessRequest(AssetRequest request)
        {
            var fileName = request.RelativePath.ExtractPattern(Settings.Regex.FileName);
            var fileNames = GetFileNames();
            foreach (var allowed in fileNames)
            {
                if (!string.IsNullOrEmpty(allowed) && !fileName.IsMatch(allowed))
                {
                    request.Context.RedirectBad("~/" + request.RelativePath);
                }
            }
            var ignore = GetIgnore();
            foreach (var ignored in ignore)
            {
                if (!string.IsNullOrEmpty(ignored) && fileName.IsMatch(ignored))
                {
                    request.Context.RedirectIgnored("~/" + request.RelativePath);
                }
            }
            if (!GetTargets().Any(x => CheckTarget(request.RelativePath, x)))
            {
                request.Context.RedirectBad("~/" + request.RelativePath);
            }
            base.ProcessRequest(request);
        }
    }
}
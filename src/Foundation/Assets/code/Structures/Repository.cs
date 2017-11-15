using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Assets.Paths.Folder;
using Lotus.Foundation.Extensions;
using Sitecore.Configuration;

namespace Lotus.Foundation.Assets.Structures
{
    internal class Repository
    {
        internal readonly Dictionary<string, IAssetPath> AssetPathByExtension = new Dictionary<string, IAssetPath>();
        internal readonly Dictionary<string, IAssetPath> AssetPathByFolder = new Dictionary<string, IAssetPath>();
        internal readonly List<IAssetPath> AssetPaths = new List<IAssetPath>();

        internal Repository(XmlNode configNode)
        {
            var pathsNode = configNode["paths"];

            if (pathsNode != null)
            {
                var paths = pathsNode.ChildNodes.OfType<XmlElement>()
                    .Select(Factory.CreateObject<IAssetPath>)
                    .ToArray();

                foreach (var path in paths)
                {
                    var key = path.GetKey();
                    if (string.IsNullOrEmpty(key))
                        continue;
                    if (key.StartsWith("extension.", StringComparison.InvariantCultureIgnoreCase))
                    {
                        AssetPathByExtension.Add(key, path);
                    }
                    if (key.StartsWith("folder.", StringComparison.InvariantCultureIgnoreCase))
                    {
                        AssetPathByFolder.Add(key, path);
                    }
                    if (key.StartsWith("file.", StringComparison.InvariantCultureIgnoreCase))
                    {
                        AssetPathByFolder.Add(key, path);
                    }
                    AssetPaths.Add(path);
                }
            }
        }

        internal IEnumerable<IAssetPath> GetPaths()
        {
            return AssetPaths;
        }

        internal IAssetPath GetExtensionPathByExtension(string extension)
        {
            return AssetPaths.FirstOrDefault(x => x.GetTargets().Contains(extension));
        }

        internal IAssetPath GetExtensionPathByKey(string key)
        {
            return AssetPathByExtension.TryGetValueOrDefault(key);
        }

        internal IAssetPath GetFolderPathByUri(string uri)
        {
            foreach (var path in AssetPaths.Where(x => x.GetKey().StartsWith("folder.")))
            {
                foreach (var folderPath in path.GetTargets().Where(x => x.StartsWith("~/")))
                {
                    if (uri.StartsWith(folderPath.Replace("~/", "")))
                    {
                        return path;
                    }
                }
                
                foreach (var folderPath in path.GetTargets().Where(x => !x.StartsWith("~/")))
                {
                    if (uri.Contains(folderPath))
                    {
                        return path;
                    }
                }
            }
            return null;
        }
        
        internal IAssetPath GetFolderPathByKey(string key)
        {
            return AssetPathByFolder.TryGetValueOrDefault(key);
        }
    }
}

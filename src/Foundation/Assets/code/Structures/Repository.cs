using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Lotus.Foundation.Assets.Paths;
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
                    .Select<XmlElement, IAssetPath>(new Func<XmlElement, IAssetPath>(Factory.CreateObject<IAssetPath>))
                    .ToArray<IAssetPath>();

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
                    AssetPaths.Add(path);
                }
            }
        }

        internal IAssetPath GetPathByExtension(string extension)
        {
            var path = default(IAssetPath);
            AssetPathByExtension.TryGetValue(extension, out path);
            return path;
        }

        internal IAssetPath GetPathByFolder(string folder)
        {
            var path = default(IAssetPath);
            AssetPathByFolder.TryGetValue(folder, out path);
            return path;
        }
    }
}

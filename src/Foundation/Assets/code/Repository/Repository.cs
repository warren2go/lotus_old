using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.String;
using Sitecore.Configuration;

namespace Lotus.Foundation.Assets.Repository
{
    internal class Repository : IAssetRepository
    {
        readonly Dictionary<string, IAssetPath> AssetPathByExtension = new Dictionary<string, IAssetPath>();
        readonly Dictionary<string, IAssetPath> AssetPathByFolder = new Dictionary<string, IAssetPath>();
        readonly Dictionary<string, IAssetPath> AssetPathByFileName = new Dictionary<string, IAssetPath>();
        readonly List<IAssetPath> AssetPaths = new List<IAssetPath>();

        protected void MapPath(XmlNode pathNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) pathNode,
                "Bad path node detected in lotus.assets.repository! Check your App_Config/Include/Lotus.Foundation.Assets.config?");
            
            var path = Factory.CreateObject<IAssetPath>(pathNode);
            
            var key = path.GetKey();
            
            if (string.IsNullOrEmpty(key))
            {
                AssetsLogger.Error("Key not specified on path:\n{0}".FormatWith(pathNode.ToString()));
                return;
            }
            
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
                AssetPathByFileName.Add(key, path);
            }
            
            AssetPaths.Add(path);
        }
        
        public IEnumerable<IAssetPath> GetPaths()
        {
            return AssetPaths;
        }

        public IAssetPath GetExtensionPathByExtension(string extension)
        {
            return AssetPaths.FirstOrDefault(x => x.GetTargets().Contains(extension));
        }

        public IAssetPath GetExtensionPathByKey(string key)
        {
            return AssetPathByExtension.TryGetValueOrDefault(key);
        }

        public IAssetPath GetFolderPathByRelativePath(string relativePath)
        {
            foreach (var path in AssetPaths.Where(x => x.GetKey().StartsWith("folder.")))
            {
                foreach (var folderPath in path.GetTargets().Where(x => x.StartsWith("~/")))
                {
                    if (relativePath.StartsWith(folderPath.Replace("~/", "")))
                    {
                        return path;
                    }
                }
                
                foreach (var folderPath in path.GetTargets().Where(x => !x.StartsWith("~/")))
                {
                    if (relativePath.Contains(folderPath))
                    {
                        return path;
                    }
                }
            }
            return null;
        }
        
        public IAssetPath GetFolderPathByKey(string key)
        {
            return AssetPathByFolder.TryGetValueOrDefault(key);
        }

        public IAssetPath GetFilePathByFileName(string fileName)
        {
            return AssetPaths.FirstOrDefault(x => x.GetTargets().Contains(fileName));
        }

        public IAssetPath GetFilePathByKey(string key)
        {
            return AssetPathByFolder.TryGetValueOrDefault(key);
        }
    }
}

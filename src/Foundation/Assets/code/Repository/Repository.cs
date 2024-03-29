﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.Serialization;

namespace Lotus.Foundation.Assets.Repository
{
    internal class Repository : IAssetRepository
    {
        public IList<string> Hosts { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IList<IAssetPath> Paths { get; set; }
        public IDictionary<string, string> MimeMapping { get; set; }
        public IDictionary<string, IAssetPath> PathByExtension { get; set; }
        public IDictionary<string, IAssetPath> PathByFolder { get; set; }
        public IDictionary<string, IAssetPath> PathByFileName { get; set; }

        public Repository()
        {
            Hosts = new List<string>();
            Headers = new Dictionary<string, string>();
            MimeMapping = new Dictionary<string, string>();
            Paths = new List<IAssetPath>();
            PathByExtension = new Dictionary<string, IAssetPath>();
            PathByFolder = new Dictionary<string, IAssetPath>();
            PathByFileName = new Dictionary<string, IAssetPath>();
        }
        
        public void MapHost(XmlNode hostNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) hostNode,
                "Bad host node detected in lotus.assets.repository! Check your App_Config/Include/Lotus.Foundation.Assets.config?");

            var host = hostNode.CastFromInnerText<string>();
            if (!string.IsNullOrEmpty(host))
            {
                Hosts.Add(host);
            }
        }

        public void MapMime(XmlNode mimeNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) mimeNode,
                "Bad host node detected in lotus.assets.repository! Check your App_Config/Include/Lotus.Foundation.Assets.config?");

            var extension = mimeNode.GetAttributeAndCast<string>("extension");
            var type = mimeNode.GetAttributeAndCast<string>("type");

            if (!string.IsNullOrEmpty(extension) && !string.IsNullOrEmpty(type))
            {
                MimeMapping.Add(extension, type);
            }
        }
        
        public void MapHeader(XmlNode headerNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) headerNode,
                "Bad header node detected in lotus.assets.repository! Check your App_Config/Include/Lotus.Foundation.Assets.config?");
            
            var name = headerNode.GetAttribute("name");
            var value = headerNode.GetAttribute("value");
            
            Headers.Add(name, value);
        }
        
        public void MapPath(XmlNode pathNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) pathNode,
                "Bad path node detected in lotus.assets.repository! Check your App_Config/Include/Lotus.Foundation.Assets.config?");
            
            var path = pathNode.ToObject<IAssetPath>();
            var key = path.GetKey();
            
            if (string.IsNullOrEmpty(key))
            {
                Global.Logger.Error("Key not specified on path:{0}{1}".FormatWith(Environment.NewLine, pathNode.ToString()));
                return;
            }
            
            if (key.StartsWith("extension.", StringComparison.InvariantCultureIgnoreCase))
            {
                PathByExtension.Add(key, path);
            }
            
            if (key.StartsWith("folder.", StringComparison.InvariantCultureIgnoreCase))
            {
                PathByFolder.Add(key, path);
            }
            
            if (key.StartsWith("file.", StringComparison.InvariantCultureIgnoreCase))
            {
                PathByFileName.Add(key, path);
            }
            
            Paths.Add(path);
        }

        public IList<string> GetHosts()
        {
            return Hosts;
        }

        public IDictionary<string, string> GetHeaders()
        {
            return Headers;
        }

        public IList<IAssetPath> GetPaths()
        {
            return Paths;
        }

        public IAssetPath GetExtensionPathByExtension(string extension)
        {
            return Paths.FirstOrDefault(x => x.GetTargets().Contains(extension));
        }

        public IAssetPath GetExtensionPathByKey(string key)
        {
            return PathByExtension.TryGetValueOrDefault(key);
        }

        public IAssetPath GetFolderPathByRelativePath(string relativePath)
        {
            foreach (var path in Paths.Where(x => x.GetKey().StartsWith("folder.")))
            {
                if (path.GetTargets().Where(x => x.StartsWith("~/")).Any(folderPath => relativePath.StartsWith(folderPath.Replace("~/", ""))))
                {
                    return path;
                }

                if (path.GetTargets().Where(x => !x.StartsWith("~/")).Any(relativePath.Contains))
                {
                    return path;
                }
            }
            return null;
        }
        
        public IAssetPath GetFolderPathByKey(string key)
        {
            return PathByFolder.TryGetValueOrDefault(key);
        }

        public IAssetPath GetFilePathByFileName(string fileName)
        {
            return Paths.FirstOrDefault(x => x.GetTargets().Contains(fileName));
        }

        public IAssetPath GetFilePathByKey(string key)
        {
            return PathByFolder.TryGetValueOrDefault(key);
        }
    }
}

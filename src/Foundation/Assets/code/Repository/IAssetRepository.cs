using System.Collections.Generic;
using System.Xml;
using Lotus.Foundation.Assets.Paths;

namespace Lotus.Foundation.Assets.Repository
{
    public interface IAssetRepository
    {
        IList<string> Hosts { get; set; }
        IDictionary<string, string> Headers { get; set; }
        IDictionary<string, string> MimeMapping { get; set; }
        IList<IAssetPath> Paths { get; set; }
        IDictionary<string, IAssetPath> PathByExtension { get; set; }
        IDictionary<string, IAssetPath> PathByFolder { get; set; }
        IDictionary<string, IAssetPath> PathByFileName { get; set; }
        
        void MapHost(XmlNode hostNode);
        void MapMime(XmlNode mimeNode);
        void MapHeader(XmlNode headerNode);
        void MapPath(XmlNode pathNode);

        IList<string> GetHosts();
        IDictionary<string, string> GetHeaders();
        IList<IAssetPath> GetPaths();
        
        IAssetPath GetExtensionPathByExtension(string extension);
        IAssetPath GetExtensionPathByKey(string key);
        IAssetPath GetFolderPathByRelativePath(string relativePath);
        IAssetPath GetFolderPathByKey(string key);
        IAssetPath GetFilePathByFileName(string fileName);
        IAssetPath GetFilePathByKey(string key);
    }
}
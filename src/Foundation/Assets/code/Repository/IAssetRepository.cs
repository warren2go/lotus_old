using System.Collections.Generic;
using System.Xml;
using Lotus.Foundation.Assets.Paths;

namespace Lotus.Foundation.Assets.Repository
{
    public interface IAssetRepository
    {
        IEnumerable<IAssetPath> GetPaths();
        IAssetPath GetExtensionPathByExtension(string extension);
        IAssetPath GetExtensionPathByKey(string key);
        IAssetPath GetFolderPathByRelativePath(string relativePath);
        IAssetPath GetFolderPathByKey(string key);
        IAssetPath GetFilePathByFileName(string fileName);
        IAssetPath GetFilePathByKey(string key);
    }
}
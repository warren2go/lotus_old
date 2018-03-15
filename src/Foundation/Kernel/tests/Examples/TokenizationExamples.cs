using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Services;
using Sitecore;

namespace Lotus.Foundation.Kernel.Tests.Examples
{
    public class TokenizationExamples
    {
        private const string _replaceExample = @"[$(line)] The home items path is '$(home).Paths.FullPath' while its DisplayName is '$(home).DisplayName'.";
        
        /// <summary>
        /// Example of a tokenization replace that uses the Sitecore context to perform the resolution of tokens and token elements.
        /// </summary>
        public void Tokenization_ReplaceExample1()
        {
            TokenContext.Add("home", Sitecore.Context.Database.GetItem("/sitecore/content/Home"));
            TokenContext.Add("line", 99);

            var format = _replaceExample;
            var resolved = TokenContext.Replace(format);
            
            System.Diagnostics.Debug.WriteLine("Resolved: {0}".FormatWith(resolved));
        }
    }
}
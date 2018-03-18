namespace Lotus.Foundation.Kernel
{
    internal static class Settings
    {
        internal static class Tokenization
        {
            internal static string TokenCharacters
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Kernel.Tokenization.TokenCharacters", "[a-zA-Z0-9_]");
                }
            }
            
            internal static string TokenElementCharacters
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Kernel.Tokenization.TokenElementCharacters", "[a-zA-Z0-9_.()]");
                }
            }

            internal static string TokenContextKey
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Kernel.Tokenization.TokenContextKey", "lotus-context-tokenizer");
                }
            }
            
            internal static string TokenFormat
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Kernel.Tokenization.TokenFormat", "$({0})");
                }
            }
            
            internal static string TokenElementFormat
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Kernel.Tokenization.TokenElementFormat", ".{0}");
                }
            }

            internal static class Security
            {
                internal static bool CheckMethods
                {
                    get
                    {
                        return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.Kernel.Tokenization.Security.CheckMethods", true);
                    }
                }
            }
        }
    }
}
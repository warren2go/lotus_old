using System;
using Lotus.Foundation.Kernel.Structures.Collections;

namespace Lotus.Foundation.Kernel.Structures
{
    public class Tokenizer : IDisposable
    {
        private const string _tokenFormat = "$({0})";
        private StaticDictionary<string, object> _tokens = new StaticDictionary<string, object>();
        
        public bool Disposed;

        public void Dispose()
        {
            Disposed = true;

            if (_tokens != null)
            {
                _tokens.Dispose();
            }
            _tokens = null;
        }

        public void Add(string key, object token)
        {
            if (key.IsMatch(@"\$\((.*)\)"))
                key = key.ExtractPattern(@"\$\((.*)\)");
            _tokens.Add(key, token);
        }

        public string Resolve(string format)
        {
            foreach (var token in _tokens)
            {
                format = format.Replace(_tokenFormat.FormatWith(token.Key), token.Value.ToString());
            }
            return format;
        }
    }
}
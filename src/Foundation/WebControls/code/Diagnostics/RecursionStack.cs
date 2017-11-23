using System;
using System.Collections;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.WebControls.Diagnostics
{
    internal class RecursionStack : IDisposable
    {
        private readonly DisposeHelper _helper = new DisposeHelper(false);
        private const string ItemsKey = "RecursionStack";
        private const string IdentifierDivider = ":::";
        private const string DetailDivider = "###";
    
        public RecursionStack(string caller, string identifier, string details)
        {
          this.Push(this.GetKey(caller, identifier, details));
        }
    
        private Stack GetStack()
        {
          Stack stack = Sitecore.Context.Items[nameof (RecursionStack)] as Stack;
          if (stack == null)
          {
            stack = new Stack();
            Sitecore.Context.Items[nameof (RecursionStack)] = (object) stack;
          }
          return stack;
        }
    
        private void Push(string key)
        {
          this.GetStack().Push((object) key);
        }
    
        public void Dispose()
        {
          if (this._helper.Disposed)
            return;
          this.Pop();
        }
    
        private void Pop()
        {
          this.GetStack().Pop();
        }
    
        public bool Contains(string caller, string details)
        {
          string key = this.GetKey(caller, details);
          foreach (string str in this.GetStack())
          {
            if (str.StartsWith(key, StringComparison.Ordinal))
              return true;
          }
          return false;
        }
    
        public int GetCount(string caller, string details)
        {
          int num = 0;
          string key = this.GetKey(caller, details);
          foreach (string str in this.GetStack())
          {
            if (str.StartsWith(key, StringComparison.Ordinal))
              ++num;
          }
          return num;
        }
    
        public string GetTrail(string caller, string divider)
        {
          string str1 = string.Empty;
          string key = this.GetKey(caller);
          foreach (string str2 in this.GetStack())
          {
            if (str2.StartsWith(key, StringComparison.Ordinal))
              str1 = divider + str2.Substring(key.Length).Replace("###", " ") + str1;
          }
          if (str1.Length > 0)
            str1 = str1.Substring(divider.Length);
          return str1;
        }
    
        private string GetKey(string caller)
        {
          return caller + ":::";
        }
    
        private string GetKey(string caller, string details)
        {
          return this.GetKey(caller) + details + "###";
        }
    
        private string GetKey(string caller, string identifier, string details)
        {
          return this.GetKey(caller, identifier) + details;
        }
    }
}
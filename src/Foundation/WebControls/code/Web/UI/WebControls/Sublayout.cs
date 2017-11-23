using Sitecore.Common;
using Sitecore.Diagnostics.PerformanceCounters;
using Sitecore.Exceptions;
using Sitecore.Layouts;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using Lotus.Foundation.WebControls.Diagnostics;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Sitecore.Web.UI.WebControls;

namespace Lotus.Foundation.WebControls.Web.UI.WebControls
{
    public class Sublayout : Sitecore.Web.UI.WebControls.Sublayout
    {
        private string _path = string.Empty;
        private Control _userControl;
    
        [Description("Path to the sublayout (.ascx file)")]
        [Category("Page")]
        [Editor(typeof (UrlEditor), typeof (UITypeEditor))]
        public string Path
        {
          get
          {
            return this._path;
          }
          set
          {
            Assert.ArgumentNotNull((object) value, nameof (value));
            this._path = value;
          }
        }
    
        public override string ID
        {
          get
          {
            return base.ID;
          }
          set
          {
            if (this._userControl != null)
            {
              base.ID = "subl_" + value;
              this._userControl.ID = value;
            }
            else
              base.ID = value;
          }
        }
    
        public override string GetTraceName()
        {
          return "Sublayout: " + this.Path;
        }
    
        public Control GetUserControl()
        {
          PageContext page1 = Sitecore.Context.Page;
          if (page1 == null)
            return (Control) null;
          Page page2 = page1.Page;
          if (page2 == null)
            return (Control) null;
          return this.GetUserControl(page2);
        }
    
        private RecursionStack CreateCyclePreventer()
        {
          Placeholder currentValue = Switcher<Placeholder, PlaceholderSwitcher>.CurrentValue;
          RecursionStack recursionStack = new RecursionStack(nameof (Sublayout), this.Path, currentValue != null ? "[" + currentValue.ContextKey + "]" : string.Empty);
          if (recursionStack.GetCount(nameof (Sublayout), this.Path) > 1)
          {
            if (Sitecore.Context.PageDesigner.IsDesigning)
            {
              HttpContext current = HttpContext.Current;
              if (current != null)
              {
                UrlString urlString = new UrlString("/sitecore/shell/Applications/WebEdit/PageDesignerError.aspx");
                urlString["sc_de"] = Sitecore.Context.PageDesigner.PageDesignerHandle;
                urlString["sc_rl"] = !string.IsNullOrEmpty(WebUtil.GetQueryString("sc_ruid")) ? "1" : "0";
                current.Response.Redirect(urlString.ToString(), true);
                return recursionStack;
              }
            }
            throw new CyclicSublayoutException("A sublayout has been recursively embedded within itself. Embedding trail: " + recursionStack.GetTrail(nameof (Sublayout), " --> "));
          }
          return recursionStack;
        }
    
        private Control GetUserControl(Page page)
        {
          Assert.ArgumentNotNull((object) page, nameof (page));
          if (this._userControl != null)
            return this._userControl;
          if (string.IsNullOrEmpty(this.Path))
            return (Control) null;
          using (new ProfileSection("Loading user control \"" + this.Path + "\""))
          {
            this._userControl = page.LoadControl(this.Path);
            if (this._userControl == null)
            {
              Tracer.Error((object) ("Could not load user control \"" + this.Path + "\""));
              return (Control) null;
            }
            UserControl userControl = this._userControl as UserControl;
            if (userControl != null)
            {
              userControl.Attributes["sc_parameters"] = this.Parameters;
              userControl.Attributes["sc_datasource"] = this.DataSource;
            }
          }
          return this._userControl;
        }
    
        public void Expand()
        {
          this.EnsureChildControls();
        }
    
        public List<Placeholder> GetPlaceholders()
        {
          Control userControl = this.GetUserControl();
          if (userControl == null)
            return (List<Placeholder>) null;
          return Placeholder.GetPlaceholders(userControl, false);
        }
    
        protected override void CreateChildControls()
        {
          base.CreateChildControls();
          string cachedHtml = this.GetCachedHtml(this.GetCacheKey());
          if (cachedHtml != null)
          {
            Tracer.Info((object) ("Rendering sub layout from cache: '" + this.Path + "'."));
            Control child = (Control) new Sublayout.NamingContainerLiteral(cachedHtml);
            this.Controls.Add(child);
            this._userControl = child;
          }
          else
          {
            using (new ProfileSection("Expanding sublayout \"" + this.Path + "\""))
            {
              using (this.CreateCyclePreventer())
              {
                Control userControl = this.GetUserControl();
                if (userControl == null)
                  return;
                PageContext page = Sitecore.Context.Page;
                if (page != null)
                  page.Expand(userControl);
                this.Controls.Add(userControl);
              }
            }
          }
        }
    
        protected override void DoRender(HtmlTextWriter output)
        {
          Assert.ArgumentNotNull((object) output, nameof (output));
          this.RenderChildren(output);
          RenderingCounters.SublayoutsRendered.Increment();
        }
    
        protected override string GetCachingID()
        {
          return this.Path;
        }
    
        protected override void OnLoad(EventArgs e)
        {
          this.OnInit(e);
          this.EnsureChildControls();
        }
    
        private class NamingContainerLiteral : LiteralControl, INamingContainer
        {
          public NamingContainerLiteral(string text)
            : base(text)
          {
          }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Primitives;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Sitecore.Web.UI.WebControls;

namespace Lotus.Foundation.Extensions.SitecoreExtensions.Rendering
{
    public static class RenderingExtensions
    {
        public static HtmlString Render(this Item item, string fieldName, object parameters = null)
        {
            return new HtmlString("");
        }

        public static HtmlString Render(this Field field, object parameters = null)
        {
            return new HtmlString("");
        }
        
        public static Item GetContextItem(this RenderingContext renderingContext)
        {
            return renderingContext.ContextItem;
        }
    }
}
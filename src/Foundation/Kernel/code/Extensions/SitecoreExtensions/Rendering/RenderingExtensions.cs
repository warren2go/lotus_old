using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lotus.Foundation.Kernel.Extensions.Casting;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Sitecore.Web.UI.WebControls;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions.Rendering
{
    public static class RenderingExtensions
    {
        //todo: this is likely not the most elegant solution, but this will preserve the pipeline/events when rendering
        private static readonly SitecoreHelper _sitecoreHelper = new SitecoreHelper(new HtmlHelper(new ViewContext(), new ViewPage()));

        public static HtmlString Render(this Item item, string fieldName, object parameters = null)
        {
            if (item.Fields[fieldName] == null)
                return new HtmlString(string.Empty);
            return item.Fields[fieldName].Render(parameters);
        }

        public static HtmlString Render(this Field field, object parameters = null)
        {
            return _sitecoreHelper.Field(field.Name, field.Item, parameters);
        }

        public static Item GetContextItem(this RenderingContext renderingContext)
        {
            return renderingContext.ContextItem;
        }

        public static Item GetPageItem(this PageContext pageContext)
        {
            return pageContext.Item;
        }
    }
}
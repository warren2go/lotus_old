﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Primitives;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Extensions.Serialization
{
    public static class XmlExtensions
    {
        public static XmlNode ByElementPath(this XmlNode parentNode, string path)
        {
            try
            {
                if (!parentNode.HasChildNodes)
                    return null;
                var node = parentNode;
                foreach (var element in path.Split('/'))
                {
                    node = node.ByElementName(element);
                }
                return node;
            }
            catch (Exception exception)
            {
                Log.Error("Error getting element by path [{0}]".FormatWith(path), exception, typeof(XmlExtensions));
                return null;
            }
        }
        
        public static XmlNode ByElementName(this XmlNode parentNode, string elementName)
        {
            try
            {
                if (!parentNode.HasChildNodes)
                    return null;
                return parentNode[elementName];
            }
            catch (Exception exception)
            {
                Log.Error("Error getting element by name [{0}]".FormatWith(elementName), exception, typeof(XmlExtensions));
                return null;
            }
        }
        
        public static T ToObject<T>(this XmlNode node) where T : class
        {
            return Factory.CreateObject<T>(node);
        }

        public static IEnumerable<T> ToObject<T>(this IEnumerable<XmlNode> nodes) where T : class
        {
            return nodes.Select(Factory.CreateObject<T>);
        }

        public static T CastFromInnerText<T>(this XmlNode node)
        {
            return node.InnerText.CastTo<T>();
        }
        
        public static T CastFromValue<T>(this XmlNode node)
        {
            return node.Value.CastTo<T>();
        }
    }
}
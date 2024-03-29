﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Primitives;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Xml;

namespace Lotus.Foundation.Extensions.Serialization
{
    public static class XmlExtensions
    {
        public static bool HasAttribute(this XmlNode node, string attributeName)
        {
            return XmlUtil.HasAttribute(attributeName, node);
        }
        
        public static XmlNode ResolvePath(this XmlNode parentNode, string path)
        {
            var lastNode = string.Empty;
            try
            {
                if (!parentNode.HasChildNodes)
                    return null;
                var node = parentNode;
                lastNode = node.LocalName;
                foreach (var element in path.Split('/'))
                {
                    node = node.GetChildElement(element);
                    lastNode = node.LocalName;
                }
                return node;
            }
            catch (Exception exception)
            {
                Log.Error("Error getting element by path - {0} [{1}]".FormatWith(lastNode, path), exception, typeof(XmlExtensions));
                return null;
            }
        }
        
        public static XmlNode GetChildElement(this XmlNode parentNode, string elementName)
        {
            try
            {
                if (!parentNode.HasChildNodes)
                    return null;
                return XmlUtil.GetChildElement(elementName, parentNode);
            }
            catch (Exception exception)
            {
                Log.Error("Error getting element by name [{0}]".FormatWith(elementName), exception, typeof(XmlExtensions));
                return null;
            }
        }
        
        public static IEnumerable<XmlNode> GetChildElements(this XmlNode parentNode, string elementName, bool allowNull = true)
        {
            try
            {
                if (!parentNode.HasChildNodes)
                    return allowNull ? null : new List<XmlNode>();
                return XmlUtil.GetChildElements(elementName, parentNode);
            }
            catch (Exception exception)
            {
                Log.Error("Error getting elements by name [{0}]".FormatWith(elementName), exception, typeof(XmlExtensions));
                return allowNull ? null : new List<XmlNode>();
            }
        }

        public static string GetAttribute(this XmlNode node, string attributeName, string defaultValue = null)
        {
            try
            {
                if (node.Attributes == null)
                    return defaultValue;
                return XmlUtil.GetAttribute(attributeName, node, defaultValue ?? string.Empty);
            }
            catch (Exception exception)
            {
                Log.Error("Error getting attribute by name [{0}]".FormatWith(attributeName), exception, typeof(XmlExtensions));
                return defaultValue;
            }
        }
        
        public static T GetAttributeAndCast<T>(this XmlNode node, string attributeName, T defaultValue = default(T))
        {
            try
            {
                if (node.Attributes == null)
                    return defaultValue;
                var value = XmlUtil.GetAttribute(attributeName, node, string.Empty);;
                return !string.IsNullOrEmpty(value) ? value.CastTo<T>() : defaultValue;
            }
            catch (Exception exception)
            {
                Log.Error("Error getting attribute by name and casting [{0} as {1}]".FormatWith(attributeName, typeof(T)), exception, typeof(XmlExtensions));
                return defaultValue;
            }
        }
        
        public static T ToObject<T>(this XmlNode node, bool assert = true) where T : class
        {
            try
            {
                return Factory.CreateObject<T>(node);
            }
            catch (Exception exception)
            {
                Log.Error("Error casting nodes to object [{0} as {1}]".FormatWith(node.LocalName, typeof(T)), exception, typeof(XmlExtensions));
                if (assert)
                {
                    throw; 
                }
                return null;
            }
        }

        public static IEnumerable<T> ToObject<T>(this IEnumerable<XmlNode> nodes, bool assert = true) where T : class
        {
            try
            {
                return nodes.Select(Factory.CreateObject<T>);
            }
            catch (Exception exception)
            {
                Log.Error("Error casting nodes to object [{0}]".FormatWith(typeof(T)), exception, typeof(XmlExtensions));
                if (assert)
                {
                    throw; 
                }
                return null;
            }
        }

        public static T CastFromAttribute<T>(this XmlNode node, string attributeName)
        {
            var attribute = node.GetAttribute(attributeName);
            return !string.IsNullOrEmpty(attribute) ? attribute.CastTo<T>() : default(T);
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
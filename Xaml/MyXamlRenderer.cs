using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

using System.Xml;
using System.Reflection;
using Spencen.Mobile.Converters;
using Spencen.Mobile.Markup;

namespace ListBoxExSample.Xaml
{
    public class XamlRenderer
    {
        //private IDrawingHost _host;
        private readonly IDictionary<Type, IConverter> _converters;
        private object _dataContext;
        private readonly IDictionary<string, object> _resourceDictionary;

        public XamlRenderer(/*IDrawingHost host,*/ object dataContext)
        {
            //_host = host;
            _dataContext = dataContext;
            DefaultNamespace = "Spencen.Mobile.UI.Primitives";
            DefaultAssemblyName = "Spencen.Mobile.UI";
            _converters = new Dictionary<Type, IConverter>();
            _resourceDictionary = new Dictionary<string, object>();

            // This is awful - but will do for now - dynamically load all type converters declared in this assembly and 
            // associate with the type that they convert. Makes heaps of non-extensible assumptions.
            var converters =
                from s in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                where s.BaseType != null && s.BaseType.IsGenericType && s.BaseType.GetGenericTypeDefinition() == typeof(Converter<>) && !s.IsInterface && !s.IsAbstract
                select new KeyValuePair<Type, IConverter>(s.BaseType.GetGenericArguments()[0], (IConverter)Activator.CreateInstance(s));

            foreach (var converter in converters)
                _converters.Add(converter);
        }

        public object FindResource(string resourceName)
        {
            return _resourceDictionary[resourceName];
        }

        public XamlElement Render(System.IO.Stream stream)
        {
            var xmlReader = new XmlTextReader(stream);
            var xamlReader = new XamlReader();
            var rootElement = xamlReader.Read(xmlReader);

            var res = Render(null, rootElement);
            return rootElement;
        }

        public object Render(object parentInstance, XamlElement rootElement)
        {
            var drawingElement = GetInstanceForMarkupElement(parentInstance, rootElement);
            //var drawingContainer = drawingElement as DrawingContainer;

            foreach (var childElement in rootElement.Children)
            {
                if (childElement.IsMember)
                {
                    // Must be a property or event
                    var propertyName = childElement.MemberName;
                    foreach (var subChildElement in childElement.Children)
                    {
                        var propertyValue = Render(drawingElement, subChildElement);
                        SetProperty(drawingElement, propertyName, propertyValue, subChildElement.ResourceKey);
                    }
                }
                    /*
                else if (drawingContainer != null)
                {
                    // Must be a child element
                    var childDrawingElement = (DrawingElement)Render(drawingElement, childElement);
                }*/
            }
            return drawingElement;
        }

        public string DefaultNamespace { get; set; }
        public string DefaultAssemblyName { get; set; }

        private object GetInstanceForMarkupElement(object parentInstance, XamlElement rootElement)
        {
            var instance = CreateInstance(parentInstance, rootElement);

            rootElement.Instance = instance;

            /*
            if (instance is DrawingElement && parentInstance is DrawingContainer)
                ((DrawingContainer)parentInstance).Children.Add((DrawingElement)instance);
            */
            if (rootElement.IsNamed)
                _resourceDictionary.Add(rootElement.ResourceName, instance);

            SetProperties(parentInstance, instance, rootElement);
            return instance;
        }

        private void SetProperties(object parentInstance, object instance, XamlElement rootElement)
        {
            //var drawingElement = parentInstance as DrawingElement;
            foreach (var property in rootElement.Attributes)
                SetProperty(instance, property);
        }

        private void SetProperty(object instance, XamlAttribute elementProperty)
        {
            SetProperty(instance, elementProperty.Name, elementProperty.Value, null);
        }

        private void SetProperty(object instance, string propertyName, object value, string key)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("elementProperty");

            var property = TypeDescriptor.GetProperties(instance, true).Find(propertyName, false);

            if (property == null)
                throw new ArgumentException(string.Format("Property {0} not found on {1}.", propertyName, instance.GetType().Name), "elementProperty");

            object convertedValue = GetPropertyValue(instance, property.PropertyType, value);

            if (property.IsReadOnly)
            {
                var currentValue = property.GetValue(instance);
                var genericCollection = currentValue as System.Collections.IDictionary;
                if (genericCollection != null)
                {
                    if (string.IsNullOrEmpty(key))
                        throw new InvalidOperationException("Must supply a key for elements that are to be added to a dictionary.");
                    else
                        genericCollection.Add(key, value);
                }
                else
                {
                    var collection = currentValue as System.Collections.IList;
                    if (collection != null)
                        collection.Add(convertedValue);
                    else
                    {
                        // HACK to accomodate ListBoxEx
                        var objectCollection = currentValue as System.Windows.Forms.ObjectCollection;
                        if (objectCollection != null)
                        {
                            if (value is System.Windows.Forms.ListBoxExRow)
                                objectCollection.Add((System.Windows.Forms.ListBoxExRow)value);
                            else
                                throw new InvalidOperationException(string.Format("Value should be of type {0} but it is actually of type {1}",
                                    typeof(System.Windows.Forms.ListBoxExRow).Name,
                                    value.GetType().Name));
                        }
                        else
                            throw new InvalidOperationException(string.Format("Property {0} on {1} is read-only.", propertyName, instance.GetType().Name));
                    }
                }
            }
            else
                property.SetValue(instance, convertedValue);
        }

        private object ConvertedValue(object parentInstance, Type propertyType, string value)
        {
            // TODO: How to use TypeConverters in Windows Mobile!?
            // Dunno - lets roll our own :-(

            if (value == "{null}")
                return null;

            // TODO: Regex these
            if (value.StartsWith("{Binding ") && value.EndsWith("}"))
            {
                var bindingName = value.Substring(9, value.Length - 10);
                // Make this recurse property path
                var targetProperty = TypeDescriptor.GetProperties(_dataContext, true).Find(bindingName, false);
                return targetProperty.GetValue(_dataContext);
            }
            /*
            else if (value.StartsWith("{StaticResource ") && value.EndsWith("}"))
            {
                var drawingElement = parentInstance as DrawingElement;
                if (drawingElement == null)
                    throw new InvalidOperationException("StaticResource may only be used on properties of a DrawingElement.");
                var resourceKey = value.Substring(16, value.Length - 17);
                return GetResource(drawingElement, resourceKey);
            }*/

            // TODO: Get TypeConverters from somewhere
            var validConverters =
                from c in _converters
                where c.Key.IsAssignableFrom(propertyType)
                orderby c.Key.Name
                select c.Value;

            foreach (var converter in validConverters)
                if (converter.CanConvertFromString(value, propertyType))
                    return converter.ConvertFromString(value, propertyType);

            return value;
        }

        /*
        private object GetResource(DrawingElement drawingElement, string resourceKey)
        {
            if (drawingElement.Resources.ContainsKey(resourceKey))
                return drawingElement.Resources[resourceKey];
            else if (drawingElement.Parent != null)
                return GetResource(drawingElement.Parent, resourceKey);
            else
                return null;
        }*/

        private string ResolveName(XamlElement rootElement)
        {
            if (string.IsNullOrEmpty(rootElement.Namespace))
                return string.Format("{0}.{1}", DefaultNamespace, rootElement.Name);
            else if (rootElement.Namespace == @"http://mobileui.codeplex.com/v1")
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var attributes = assembly.GetCustomAttributes(typeof(XamlNamespaceMappingAttribute), true);
                foreach (XamlNamespaceMappingAttribute attribute in attributes)
                {
                    var fullTypeName = string.Format("{0}.{1}", attribute.ClrNamespace, rootElement.Name, false);
                    var resolvedType = assembly.GetType(fullTypeName);
                    if (resolvedType != null)
                        return resolvedType.FullName;
                }
            }



            int idx = rootElement.Namespace.IndexOf(',');
            string splitName, splitNamespace;
            if (idx == -1)
            {
                splitName = rootElement.Namespace;
                splitNamespace = "";
            }
            else
            {
                splitName = rootElement.Namespace.Substring(0, idx);
                splitNamespace = rootElement.Namespace.Substring(idx + 1);
            }

            return string.Format("{0}.{1},{2}",
                splitName,
                rootElement.Name,
                splitNamespace.Length == 1 ? DefaultAssemblyName : splitNamespace);
        }

        private object CreateInstance(object parentInstance, XamlElement rootElement)
        {
            var className = ResolveName(rootElement);
            var type = Type.GetType(className, true, false);

            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance);
            var hostConstructor = (from c in constructors where c.GetParameters().Count() == 0/*.Any(p => p.ParameterType == typeof(IDrawingHost))*/ select c).FirstOrDefault();
            if (hostConstructor != null)
                return hostConstructor.Invoke(new object[] { /*_host*/ });

            object[] args;
            var bestConstructor = GetBestMatchingConstructor(constructors, parentInstance, rootElement, out args);
            if (bestConstructor != null)
                return bestConstructor.Invoke(args);
            else
                return Activator.CreateInstance(type);
        }

        private ConstructorInfo GetBestMatchingConstructor(ConstructorInfo[] constructors, object parentInstance, XamlElement rootElement, out object[] args)
        {
            foreach (var constructor in constructors.OrderByDescending(c => c.GetParameters().Count()))
            {
                var matchingAttributeCount = 0;
                var parameters = constructor.GetParameters();
                foreach (var parameter in parameters)
                {
                    var matchingAttribute = rootElement.Attributes.FirstOrDefault(a => a.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                    if (matchingAttribute == null)
                        break;
                    else
                        matchingAttributeCount++;
                }
                if (matchingAttributeCount == parameters.Length) // Found them all
                {
                    args = new object[parameters.Length];
                    var argIndex = 0;
                    foreach (var parameter in parameters)
                    {
                        var matchingAttribute = rootElement.Attributes.First(a => a.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                        args[argIndex++] = GetPropertyValue(parentInstance, parameter.ParameterType, matchingAttribute.Value);

                        // Since the attribute is going to be used as a constructor argument we don't need to process it as a property.
                        rootElement.Attributes.Remove(matchingAttribute);
                    }
                    return constructor;
                }
            }
            args = new object[] { };
            return null;
        }

        private object GetPropertyValue(object parentInstance, Type type, object value)
        {
            object convertedValue = value;
            if (value is string)
                convertedValue = ConvertedValue(parentInstance, type, (string)value);

            return convertedValue;
        }

        private string GetFullTypeName(string className)
        {
            return string.Format("{0},{1}", className, DefaultAssemblyName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pathoschild.DesignByContract.Framework.Analysis
{
    /// <summary>Reflects methods and properties for contract analysis.</summary>
    [Serializable]
    public class MethodAnalyzer : IMethodAnalyzer
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The singleton instance.</summary>
        public static MethodAnalyzer Instance = new MethodAnalyzer();


        /*********
        ** Public methods
        *********/
        /// <summary>Analyze the contract annotations on a methods.</summary>
        /// <param name="method">The method to analyze.</param>
        /// <param name="inheritContract">Whether to inherit attributes from base types or interfaces.</param>
        public MethodAnalysis AnalyzeMethod(MethodBase method, bool inheritContract)
        {
            // analyze method contract
            var parameterPreconditions = this.GetParameterPreconditions(method, inheritContract);
            var returnPreconditions = this.GetReturnValuePreconditions(method, inheritContract);

            // analyze property contract
            PropertyInfo property = this.GetProperty(method);
            if (property != null)
            {
                // cascade annotations on the property to the getter/setter methods
                if (this.IsPropertyGetter(method))
                    returnPreconditions = returnPreconditions.Union(this.GetReturnValuePreconditions(property, inheritContract));
                else
                    parameterPreconditions = parameterPreconditions.Union(this.GetParameterPreconditions(property, inheritContract));
            }

            // return analysis
            return new MethodAnalysis
            {
                ParameterPreconditions = parameterPreconditions.ToArray(),
                ReturnValuePreconditions = returnPreconditions.ToArray()
            };
        }


        /*********
        ** Protected methods
        *********/
        /***
        ** Method analysis
        ***/
        /// <summary>Get whether a method returns a value.</summary>
        /// <param name="method">The method to analyze.</param>
        protected bool HasReturnValue(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            return methodInfo != null && methodInfo.ReturnType != typeof(void);
        }

        /// <summary>Get whether a method is a property getter or setter.</summary>
        /// <param name="method">The method to analyze.</param>
        protected bool IsPropertyAccessor(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            return methodInfo != null
                && methodInfo.IsSpecialName
                && (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"));
        }

        /// <summary>Get whether the method is a property setter.</summary>
        /// <param name="method">The method to analyze.</param>
        protected bool IsPropertyGetter(MethodBase method)
        {
            return this.IsPropertyAccessor(method)
                && method.Name.StartsWith("get_");
        }

        /// <summary>Get the property for which this method is an accessor.</summary>
        /// <param name="method">The method to analyze.</param>
        /// <returns>Returns the method's property, or <c>null</c> if it is not an accessor.</returns>
        protected PropertyInfo GetProperty(MethodBase method)
        {
            // analyze method
            if (!this.IsPropertyAccessor(method) || !(method is MethodInfo))
                return null;
            MethodInfo methodInfo = method as MethodInfo;
            bool isGet = this.IsPropertyGetter(methodInfo);

            // get matching property
            PropertyInfo[] properties = method.DeclaringType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(p => methodInfo == (isGet ? p.GetGetMethod(true) : p.GetSetMethod(true)))
                .ToArray();
            if (properties.Length <= 1)
                return properties.SingleOrDefault();

            // disambiguate between properties (e.g., hidden properties)
            PropertyInfo mostDerived = properties.First();
            foreach (PropertyInfo property in properties)
            {
                if (property.DeclaringType.IsSubclassOf(mostDerived.DeclaringType))
                    mostDerived = property;
            }
            return mostDerived;
        }

        /***
        ** Attributes
        ***/
        /// <summary>Get the custom attributes of a given type from a provider.</summary>
        /// <typeparam name="T">The type of the custom attributes.</typeparam>
        /// <param name="customAttributeProvider">The object from which to retrieve custom attributes.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<T> GetCustomAttributes<T>(ICustomAttributeProvider customAttributeProvider, bool inherit)
        {
            return customAttributeProvider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        /// <summary>Get the custom attributes of a given type from a provider.</summary>
        /// <typeparam name="T">The type of the custom attributes.</typeparam>
        /// <param name="property">The object from which to retrieve custom attributes.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<T> GetCustomAttributes<T>(MemberInfo property, bool inherit)
        {
            return property.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        /// <summary>Get the attributes applied to a method.</summary>
        /// <typeparam name="T">The attribute type to get.</typeparam>
        /// <param name="method">The method whose attributes to get.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        /// <param name="withReturnTypeAttributes">Whether to include attributes restricted to its return type.</param>
        protected IEnumerable<T> GetMethodAttributes<T>(MethodInfo method, bool inherit, bool withReturnTypeAttributes = false)
        {
            var attributes = this.GetCustomAttributes<T>(method, inherit);
            if (withReturnTypeAttributes)
                attributes = attributes.Union(this.GetCustomAttributes<T>(method.ReturnTypeCustomAttributes, inherit));

            return attributes;
        }

        /// <summary>Get the attributes applied to a parameter.</summary>
        /// <typeparam name="T">The attribute type to get.</typeparam>
        /// <param name="parameter">The parameter whose attributes to get.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<T> GetParameterAttributes<T>(ParameterInfo parameter, bool inherit)
        {
            return this.GetCustomAttributes<T>(parameter, inherit);
        }

        /***
        ** Preconditions
        ***/
        /// <summary>Get the contract requirements for each method parameter or property setter value.</summary>
        /// <param name="method">The method to analyze.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<ParameterMetadata> GetParameterPreconditions(MethodBase method, bool inherit)
        {
            // would have been a bit cleaner to do this in GetCustomAttributes but it'll be
            // more performant to run this once per method vs once per method param
            var interfaceMethod = GetInterfaceDefinition(method) as MethodInfo;
            if (interfaceMethod != null)
            {
                var interfaceParameters = interfaceMethod.GetParameters();
                var methodParameters = method.GetParameters();

                // in the case that both the interface and the implementation define an attribute, 
                // we assume that the implementation has defined only complementary attributes so we include
                // them all. this is a valid use case when the attributes contain parameters - the implementation
                // may define further restrictions

                return methodParameters.SelectMany(parameter =>
                {
                    var interfaceParam = interfaceParameters.Single(p => p.Position == parameter.Position);
                    return GetAnnotations(parameter, true).Union(GetAnnotations(interfaceParam, false));
                });
            }

            return method.GetParameters().SelectMany(parameter => GetAnnotations(parameter, inherit));
        }

        

        /// <summary>Get the contract requirements for the property setter value.</summary>
        /// <param name="property">The property to analyze.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<ParameterMetadata> GetParameterPreconditions(PropertyInfo property, bool inherit)
        {
            if (!property.CanWrite)
                return new ParameterMetadata[0];                   

            var interfaceProperty = GetInterfaceDefinition(property) as PropertyInfo;
            if(interfaceProperty != null)
            {
                return GetAnnotations(interfaceProperty, false)
                    .Union(GetAnnotations(property, inherit));
            }

            return GetAnnotations(property, inherit);
        }

        /// <summary>Get the contract requirements on a method or property return value.</summary>
        /// <param name="method">The method to analyze.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<ReturnValueMetadata> GetReturnValuePreconditions(MethodBase method, bool inherit)
        {
            if (!this.HasReturnValue(method))
                return new ReturnValueMetadata[0];

            MethodInfo interfaceMethod = GetInterfaceDefinition(method) as MethodInfo;
            if (inherit && interfaceMethod != null)
            {
                var methodAttributes = GetMethodAttributes<IReturnValuePrecondition>(method as MethodInfo, true, true);
                var interfaceAttributes = GetMethodAttributes<IReturnValuePrecondition>(interfaceMethod, false, true);
                return methodAttributes.Union(interfaceAttributes)
                    .Select(annotation => new ReturnValueMetadata(interfaceMethod, annotation));
            }

            return this
                .GetMethodAttributes<IReturnValuePrecondition>(method as MethodInfo, inherit, true)
                .Select(attr => new ReturnValueMetadata(method, attr));
        }

        /// <summary>Get the contract requirements on a method or property return value.</summary>
        /// <param name="property">The method to analyze.</param>
        /// <param name="inherit">Whether to inherit attributes from base types or interfaces.</param>
        protected IEnumerable<ReturnValueMetadata> GetReturnValuePreconditions(PropertyInfo property, bool inherit)
        {
            var interfaceMethod = GetInterfaceDefinition(property);
            if (inherit && interfaceMethod != null)
            {
                var methodAttributes = GetCustomAttributes<IReturnValuePrecondition>(property, true);
                var interfaceAttributes = GetCustomAttributes<IReturnValuePrecondition>(interfaceMethod, false);
                return methodAttributes.Union(interfaceAttributes)
                    .Select(annotation => new ReturnValueMetadata(interfaceMethod, annotation));
            }

            return this
                .GetCustomAttributes<IReturnValuePrecondition>(property, inherit)
                .Select(attr => new ReturnValueMetadata(property, attr));
        }

        private IEnumerable<ParameterMetadata> GetAnnotations(ParameterInfo parameter, bool inherit)
        {
            return GetParameterAttributes<IParameterPrecondition>(parameter, inherit)
                .Select(annotation => new ParameterMetadata(parameter, annotation));
        }

        private IEnumerable<ParameterMetadata> GetAnnotations(PropertyInfo property, bool inherit)
        {
            return GetCustomAttributes<IParameterPrecondition>(property, inherit)
                .Select(annotation =>
                {
                    // setter value (implicit last parameter)
                    ParameterInfo parameter = property.GetSetMethod(true).GetParameters().Last();
                    return new ParameterMetadata(parameter, annotation, property.Name);
                });
        }

        private MemberInfo GetInterfaceDefinition(MemberInfo info)
        {
            Type methodType = info.ReflectedType;
            return methodType.GetInterfaces()
                .SelectMany(iface => methodType.GetInterfaceMap(iface).InterfaceMethods)
                .Where(m => MemberSignatureEquals(m, info))
                .Select(m =>
                {
                    // if it's a prop getter/setter, return the property itself
                    if (IsPropertyAccessor(m))
                        return (MemberInfo)GetProperty(m);
                    return m;
                })
                .Distinct()
                .SingleOrDefault();
        }

        private bool MemberSignatureEquals(MemberInfo m1, MemberInfo m2)
        {
            Func<ParameterInfo, Type> selectParameterType = p => p.ParameterType;
            Func<MemberInfo, string> nameOf = mi => mi is MethodBase && IsPropertyAccessor(mi as MethodBase)
                ? GetProperty(mi as MethodBase).Name
                : mi.Name;

            string name1 = nameOf(m1);
            string name2 = nameOf(m2);

            return name1 == name2
                // ctors
                   && SelectivelyEquals<MethodBase>(m => m.GetParameters().Select(selectParameterType), m1, m2)
                // methods
                   && SelectivelyEquals<MethodInfo>(m => m.ReturnType, m1, m2)
                // fields
                   && SelectivelyEquals<FieldInfo>(f => f.FieldType, m1, m2)
                // properties
                   && SelectivelyEquals<PropertyInfo>(p => p.PropertyType, m1, m2);
        }

        private static bool SelectivelyEquals<T>(Func<T, IEnumerable<Type>> select, MemberInfo o1, MemberInfo o2)
            where T : class
        {
            if (!(o1 is T) || !(o2 is T))
                return true;
            return select(o1 as T).SequenceEqual(select(o2 as T));
        }

        private static bool SelectivelyEquals<T>(Func<T, Type> select, MemberInfo o1, MemberInfo o2)
            where T : class
        {
            if (!(o1 is T) || !(o2 is T))
                return true;
            return select(o1 as T) == select(o2 as T);
        }
    }
}

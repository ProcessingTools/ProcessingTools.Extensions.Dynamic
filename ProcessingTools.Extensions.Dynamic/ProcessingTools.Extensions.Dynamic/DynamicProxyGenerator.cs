// <copyright file="DynamicProxyGenerator.cs" company="ProcessingTools">
// Copyright (c) 2020 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Extensions.Dynamic
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Dynamic proxy generator.
    /// See http://geekswithblogs.net/abhijeetp/archive/2010/04/04/a-simple-dynamic-proxy.aspx.
    /// </summary>
    public static class DynamicProxyGenerator
    {
        /// <summary>
        /// Get fake instance for specified interface type.
        /// Fake instance means that methods and properties does not set or get data, but
        /// the instance implements the specified interface type.
        /// </summary>
        /// <typeparam name="T">Interface type to be instantiated.</typeparam>
        /// <returns>Fake instance of type T.</returns>
        /// <exception cref="InvalidOperationException">If the type T is not interface.</exception>
        public static T GetFakeInstanceFor<T>()
        {
            Type typeOfT = typeof(T);
            if (!typeOfT.IsInterface)
            {
                throw new InvalidOperationException();
            }

            var assemblyName = new AssemblyName("testAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("testModule");

            var typeBuilder = moduleBuilder.DefineType(typeOfT.Name + "Proxy", TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(typeOfT);

            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Array.Empty<Type>());
            var constructorILGenerator = constructorBuilder.GetILGenerator();
            constructorILGenerator.EmitWriteLine("Creating Proxy instance");
            constructorILGenerator.Emit(OpCodes.Ret);

            var methodInfos = typeOfT.GetMethods();
            foreach (var methodInfo in methodInfos)
            {
                var parameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
                var returnType = methodInfo.ReturnType;
                var attributes = MethodAttributes.Public | MethodAttributes.Virtual;

                var methodBuilder = typeBuilder.DefineMethod(name: methodInfo.Name, attributes: attributes, returnType: returnType, parameterTypes: parameterTypes);

                var methodILGenerator = methodBuilder.GetILGenerator();
                if (methodInfo.ReturnType == typeof(void))
                {
                    methodILGenerator.Emit(OpCodes.Ret);
                }
                else
                {
                    if (methodInfo.ReturnType.IsValueType || methodInfo.ReturnType.IsEnum)
                    {
                        Type typeOfType = typeof(Type);
                        MethodInfo methodGetTypeFromHandle = typeOfType.GetMethod(nameof(Type.GetTypeFromHandle));
                        MethodInfo methodCreateInstance = typeof(Activator).GetMethod(nameof(Activator.CreateInstance), new[] { typeOfType });

                        LocalBuilder localBuilder = methodILGenerator.DeclareLocal(methodInfo.ReturnType);

                        methodILGenerator.Emit(OpCodes.Ldtoken, localBuilder.LocalType);
                        methodILGenerator.Emit(OpCodes.Call, methodGetTypeFromHandle);
                        methodILGenerator.Emit(OpCodes.Callvirt, methodCreateInstance);
                        methodILGenerator.Emit(OpCodes.Unbox_Any, localBuilder.LocalType);
                    }
                    else
                    {
                        methodILGenerator.Emit(OpCodes.Ldnull);
                    }

                    methodILGenerator.Emit(OpCodes.Ret);
                }

                typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
            }

            Type constructedType = typeBuilder.CreateTypeInfo();
            var instance = Activator.CreateInstance(constructedType);
            return (T)instance;
        }
    }
}

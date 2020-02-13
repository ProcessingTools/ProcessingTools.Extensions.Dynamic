// <copyright file="PocoTypeBuilder.cs" company="ProcessingTools">
// Copyright (c) 2020 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Extensions.Dynamic
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Dynamic builder for POCO types.
    /// </summary>
    public class PocoTypeBuilder : IPocoTypeBuilder
    {
        private readonly TypeBuilder typeBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PocoTypeBuilder"/> class.
        /// </summary>
        /// <param name="moduleBuilder">Instance of <see cref="ModuleBuilder"/>.</param>
        public PocoTypeBuilder(ModuleBuilder moduleBuilder)
            : this(moduleBuilder, Guid.NewGuid().ToString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PocoTypeBuilder"/> class.
        /// </summary>
        /// <param name="moduleBuilder">Instance of <see cref="ModuleBuilder"/>.</param>
        /// <param name="typeName">Name of the resultant type.</param>
        public PocoTypeBuilder(ModuleBuilder moduleBuilder, string typeName)
        {
            if (moduleBuilder is null)
            {
                throw new ArgumentNullException(nameof(moduleBuilder));
            }

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            this.typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
        }

        /// <inheritdoc/>
        public void AddProperty(string propertyName, Type propertyType)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (propertyType is null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            string fieldName = "__" + propertyName + "__";

            FieldBuilder fieldBuilder = this.typeBuilder.DefineField(fieldName, propertyType, FieldAttributes.Private);

            // The last argument of DefineProperty is null, because the
            // property has no parameters. (If you don't specify null, you must
            // specify an array of Type objects. For a parameterless property,
            // use an array with no elements: new Type[] {})
            PropertyBuilder propertyBuilder = this.typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            // The property set and property get methods require a special set of attributes.
            MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            MethodBuilder customerNameGetMethodBuilder = this.CreateGetMethodBuilder(propertyName, propertyType, fieldBuilder, methodAttributes);

            MethodBuilder customerNameSetMethodBuilder = this.CreateSetMethodBuilder(propertyName, propertyType, fieldBuilder, methodAttributes);

            // Last, we must map the two methods created above to our PropertyBuilder to
            // their corresponding behaviors, "get" and "set" respectively.
            propertyBuilder.SetGetMethod(customerNameGetMethodBuilder);
            propertyBuilder.SetSetMethod(customerNameSetMethodBuilder);
        }

        /// <inheritdoc/>
        public Type CreateType()
        {
            return this.typeBuilder.CreateType();
        }

        private MethodBuilder CreateSetMethodBuilder(string propertyName, Type propertyType, FieldBuilder fieldBuilder, MethodAttributes methodAttributes)
        {
            MethodBuilder methodBuilder = this.typeBuilder.DefineMethod("set_" + propertyName, methodAttributes, null, new[] { propertyType });

            ILGenerator ilgenerator = methodBuilder.GetILGenerator();

            ilgenerator.Emit(OpCodes.Ldarg_0);
            ilgenerator.Emit(OpCodes.Ldarg_1);
            ilgenerator.Emit(OpCodes.Stfld, fieldBuilder);
            ilgenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        private MethodBuilder CreateGetMethodBuilder(string propertyName, Type propertyType, FieldBuilder fieldBuilder, MethodAttributes methodAttributes)
        {
            MethodBuilder methodBuilder = this.typeBuilder.DefineMethod("get_" + propertyName, methodAttributes, propertyType, Type.EmptyTypes);

            ILGenerator customerNameGetIL = methodBuilder.GetILGenerator();

            customerNameGetIL.Emit(OpCodes.Ldarg_0);
            customerNameGetIL.Emit(OpCodes.Ldfld, fieldBuilder);
            customerNameGetIL.Emit(OpCodes.Ret);

            return methodBuilder;
        }
    }
}

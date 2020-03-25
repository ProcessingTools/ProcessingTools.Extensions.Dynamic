// <copyright file="MapperConfigurationExtensions.cs" company="ProcessingTools">
// Copyright (c) 2020 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Extensions.Dynamic.AutoMapper
{
    using System;
    using global::AutoMapper;

    /// <summary>
    /// <see cref="MapperConfiguration"/> extensions.
    /// </summary>
    public static class MapperConfigurationExtensions
    {
        /// <summary>
        /// Creates a mapping configuration from the TSource type to the TDestination type with proxy type.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TDestination">Destination type.</typeparam>
        /// <param name="configuration">Instance of <see cref="IMapperConfigurationExpression"/> to be updated.</param>
        /// <returns>Mapping expression for more configuration options.</returns>
        public static IMappingExpression<TSource, TDestination> CreateMapWithProxy<TSource, TDestination>(this IMapperConfigurationExpression configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return (IMappingExpression<TSource, TDestination>)CreateMapWithProxy(configuration, sourceType: typeof(TSource), destinationType: typeof(TDestination));
        }

        /// <summary>
        /// Creates a mapping configuration from the TSource type to the TDestination type with proxy type.
        /// Specify the member list to validate against during configuration validation.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TDestination">Destination type.</typeparam>
        /// <param name="configuration">Instance of <see cref="IMapperConfigurationExpression"/> to be updated.</param>
        /// <param name="memberList">Member list to validate.</param>
        /// <returns>Mapping expression for more configuration options.</returns>
        public static IMappingExpression<TSource, TDestination> CreateMapWithProxy<TSource, TDestination>(this IMapperConfigurationExpression configuration, MemberList memberList)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return (IMappingExpression<TSource, TDestination>)CreateMapWithProxy(configuration, sourceType: typeof(TSource), destinationType: typeof(TDestination), memberList: memberList);
        }

        /// <summary>
        /// Creates a mapping configuration from the source type to the destination type with proxy type if destinationType is an interface.
        /// </summary>
        /// <param name="configuration">Instance of <see cref="IMapperConfigurationExpression"/> to be updated.</param>
        /// <param name="sourceType">Source type.</param>
        /// <param name="destinationType">Destination type.</param>
        /// <returns>Mapping expression for more configuration options.</returns>
        public static IMappingExpression CreateMapWithProxy(this IMapperConfigurationExpression configuration, Type sourceType, Type destinationType)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (sourceType is null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }

            if (destinationType is null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (destinationType.IsInterface)
            {
                Type destinationTypeProxy = DynamicProxyBuilder.ModuleBuilder.GetProxyTypeOf(destinationType);
                return configuration.CreateMap(sourceType: sourceType, destinationType: destinationTypeProxy);
            }
            else
            {
                return configuration.CreateMap(sourceType: sourceType, destinationType: destinationType);
            }
        }

        /// <summary>
        /// Creates a mapping configuration from the source type to the destination type with proxy type if destinationType is an interface.
        /// Specify the member list to validate against during configuration validation.
        /// </summary>
        /// <param name="configuration">Instance of <see cref="IMapperConfigurationExpression"/> to be updated.</param>
        /// <param name="sourceType">Source type.</param>
        /// <param name="destinationType">Destination type.</param>
        /// <param name="memberList">Member list to validate.</param>
        /// <returns>Mapping expression for more configuration options.</returns>
        public static IMappingExpression CreateMapWithProxy(this IMapperConfigurationExpression configuration, Type sourceType, Type destinationType, MemberList memberList)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (sourceType is null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }

            if (destinationType is null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (destinationType.IsInterface)
            {
                Type destinationTypeProxy = DynamicProxyBuilder.ModuleBuilder.GetProxyTypeOf(destinationType);
                return configuration.CreateMap(sourceType: sourceType, destinationType: destinationTypeProxy, memberList: memberList);
            }
            else
            {
                return configuration.CreateMap(sourceType: sourceType, destinationType: destinationType, memberList: memberList);
            }
        }
    }
}

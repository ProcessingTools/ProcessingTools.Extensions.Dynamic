// <copyright file="DynamicProxyGeneratorTests.cs" company="ProcessingTools">
// Copyright (c) 2020 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Extensions.Dynamic.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="DynamicProxyGenerator"/>.
    /// </summary>
    [TestFixture]
    public class DynamicProxyGeneratorTests
    {
        /// <summary>
        /// Model interface for tests.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Test type")]
        public interface IMyTestModel
        {
            /// <summary>
            /// Gets or sets the ID of the test model.
            /// </summary>
            int Id { get; set; }

            /// <summary>
            /// Gets or sets the name of the test model.
            /// </summary>
            string Name { get; set; }
        }

        /// <summary>
        /// Model interface for tests.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Test type")]
        public interface IMyTestModelWithGetters
        {
            /// <summary>
            /// Gets the ID of the test model.
            /// </summary>
            int Id { get; }

            /// <summary>
            /// Gets the name of the test model.
            /// </summary>
            string Name { get; }
        }

        /// <summary>
        /// <see cref="DynamicProxyGenerator"/>.GetFakeInstanceFor should work.
        /// </summary>
        [Test(TestOf = typeof(DynamicProxyGenerator))]
        public void DynamicProxyGenerator_GetFakeInstanceFor_ShouldWork()
        {
            // Arrange + Act
            IMyTestModel instance = DynamicProxyGenerator.GetFakeInstanceFor<IMyTestModel>();

            // Assert
            Assert.IsNotNull(instance);
        }

        /// <summary>
        /// <see cref="DynamicProxyGenerator"/>.GetFakeInstanceFor for model only with getters should work.
        /// </summary>
        [Test(TestOf = typeof(DynamicProxyGenerator))]
        public void DynamicProxyGenerator_GetFakeInstanceFor_OnlyGetters_ShouldWork()
        {
            // Arrange + Act
            IMyTestModelWithGetters instance = DynamicProxyGenerator.GetFakeInstanceFor<IMyTestModelWithGetters>();

            // Assert
            Assert.IsNotNull(instance);
        }

        /// <summary>
        /// <see cref="DynamicProxyGenerator"/>.GetFakeInstanceFor should return valid instance.
        /// </summary>
        [Test(TestOf = typeof(DynamicProxyGenerator))]
        public void DynamicProxyGenerator_GetFakeInstanceFor_ShouldReturnValidInstance()
        {
            // Arrange
            int id = 1;
            string name = "123123";
            IMyTestModel instance = DynamicProxyGenerator.GetFakeInstanceFor<IMyTestModel>();

            // Act
            instance.Id = id;
            instance.Name = name;

            // Assert
            Assert.AreEqual(id, instance.Id);
            Assert.AreEqual(name, instance.Name);
        }

        /// <summary>
        /// <see cref="DynamicProxyGenerator"/>.GetFakeInstanceFor for model only with getters should return valid instance.
        /// </summary>
        [Test(TestOf = typeof(DynamicProxyGenerator))]
        public void DynamicProxyGenerator_GetFakeInstanceFor_OnlyGetters_ShouldReturnValidInstance()
        {
            // Arrange
            int id = 1;
            string name = "123123";
            IMyTestModelWithGetters instance = DynamicProxyGenerator.GetFakeInstanceFor<IMyTestModelWithGetters>();

            // Act
            instance.GetType().GetProperty(nameof(IMyTestModelWithGetters.Id)).SetMethod.Invoke(instance, new object[] { id });
            instance.GetType().GetProperty(nameof(IMyTestModelWithGetters.Name)).SetMethod.Invoke(instance, new object[] { name });

            // Assert
            Assert.AreEqual(id, instance.Id);
            Assert.AreEqual(name, instance.Name);
        }
    }
}

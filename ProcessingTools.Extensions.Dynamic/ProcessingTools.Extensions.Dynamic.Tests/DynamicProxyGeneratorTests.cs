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
    }
}

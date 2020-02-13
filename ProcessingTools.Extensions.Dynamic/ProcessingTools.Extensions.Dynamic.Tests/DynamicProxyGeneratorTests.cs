// <copyright file="DynamicProxyGeneratorTests.cs" company="ProcessingTools">
// Copyright (c) 2020 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Extensions.Dynamic.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="ProcessingTools.Extensions.Dynamic.DynamicProxyGenerator"/>.
    /// </summary>
    [TestFixture]
    public class DynamicProxyGeneratorTests
    {
        /// <summary>
        /// Model interface for tests.
        /// </summary>
        private interface IMyTestModel
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
        /// <see cref="ProcessingTools.Extensions.Dynamic.DynamicProxyGenerator"/>.GetInstanceFor should work.
        /// </summary>
        [Test(TestOf = typeof(DynamicProxyGenerator))]
        public void DynamicProxyGenerator_GetInstanceFor_ShouldWork()
        {
            // Arrange + Act
            IMyTestModel instance = DynamicProxyGenerator.GetInstanceFor<IMyTestModel>();

            // Assert
            Assert.IsNotNull(instance);
        }
    }
}

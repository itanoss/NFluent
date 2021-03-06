﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FluentStructCheck.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Kernel
{
    using System.Diagnostics.CodeAnalysis;
    using Extensibility;
    using Helpers;

    /// <summary>
    /// Provides check methods to be executed on a given struct value.
    /// </summary>
    /// <typeparam name="T">Type of the struct value to assert on.</typeparam>
    internal sealed class FluentStructCheck<T> : IForkableCheck, IStructCheck<T>, ICheckForExtensibility<T, IStructCheck<T>> where T : struct
    {
        private readonly Checker<T, IStructCheck<T>> structChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentStructCheck{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public FluentStructCheck(T value) : this(value, !CheckContext.DefaulNegated)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentStructCheck{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="negated">A boolean value indicating whether the check should be negated or not.</param>
        private FluentStructCheck(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.structChecker = new Checker<T, IStructCheck<T>>(this);
        }

        /// <inheritdoc />

        public T Value { get; private set; }

        /// <inheritdoc />

        public bool Negated { get; private set; }

        /// <inheritdoc />

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public IStructCheck<T> Not
        {
            get
            {
                bool negated = CheckContext.DefaulNegated;
                return new FluentStructCheck<T>(this.Value, negated);
            }
        }

        /// <summary>
        /// Gets the runner to use for checking something on a given type.
        /// </summary>
        /// <value>
        /// The runner to use for checking something on a given type.
        /// </value>
        public IChecker<T, IStructCheck<T>> Checker => this.structChecker;

        /// <summary>
        /// Creates a new instance of the same fluent check type, injecting the same Value property
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <returns>
        /// A new instance of the same fluent check type, with the same Value property.
        /// </returns>
        /// <remarks>
        /// This method is used during the chaining of multiple checks.
        /// </remarks>
        object IForkableCheck.ForkInstance()
        {
            this.Negated = !CheckContext.DefaulNegated;
            return this;
        }

        /// <summary>
        /// Checks whether if the checked value is of the given type.
        /// </summary>
        /// <typeparam name="TU">The given type to check the checked value against.</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException">The specified value is not of the given type.</exception>
        public ICheckLink<IStructCheck<T>> IsInstanceOf<TU>() where TU : struct
        {
            return this.structChecker.ExecuteCheck(
                () => IsInstanceHelper.IsInstanceOf(this.Value, typeof(TU)), 
                IsInstanceHelper.BuildErrorMessage(this.Value, typeof(TU), true));
        }

        /// <summary>
        /// Checks whether if the checked value is different from the given type.
        /// </summary>
        /// <typeparam name="TU">The given type to check the checked value against.</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException">The specified value is of the given type.</exception>
        public ICheckLink<IStructCheck<T>> IsNotInstanceOf<TU>() where TU : struct
        {
            return this.structChecker.ExecuteCheck(
                () => IsInstanceHelper.IsNotInstanceOf(this.Value, typeof(TU)),
                IsInstanceHelper.BuildErrorMessage(this.Value, typeof(TU), false));
        }
    }
}
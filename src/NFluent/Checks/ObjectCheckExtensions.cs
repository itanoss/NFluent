﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectCheckExtensions.cs" company="">
//   Copyright 2014 Thomas PIERRAIN, Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System;
    using System.ComponentModel;
#if !DOTNET_20 && !DOTNET_30
    using System.Linq;
#endif
    using Extensibility;
    using Helpers;

    /// <summary>
    /// Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectCheckExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> IsEqualTo<T>(this ICheck<T> check, T expected)
        {
            return IsEqualTo(check, (object)expected);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> IsEqualTo<T>(this ICheck<T> check, object expected)
        {
            return EqualityHelper.PerformEqualCheck(check, expected);
        }

        /// <summary>
        /// Checks that the actual value is one of the legal values.
        /// </summary>
        /// <typeparam name="T">Type of the checked value.</typeparam>
        /// <param name="check">The fluent check context object.</param>
        /// <param name="values">List of possible values.</param>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<T>> IsOneOf<T>(this ICheck<T> check, params T[] values)
        {
            var comparer = new EqualityHelper.EqualityComparer<T>();
            ExtensibilityHelper.BeginCheck(check)
                .ExpectingValues(values, values.Length, "one of", "none of")
                .FailsIf(sut => !values.Any(value => comparer.Equals(sut, value)), "The {0} is not one of the {1}.")
                .Negates("The {0} should not be one of the {1}.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value using operator==.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <typeparam name="TU">Type of the expected value</typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> HasSameValueAs<T, TU>(this ICheck<T> check, TU expected)
        {
            return EqualityHelper.PerformEqualCheck(check, expected, true);
        }

        /// <summary>
        /// Checks that the actual value is different from another expected value using operator!=.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <typeparam name="TU">Type of the expected value.</typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> HasDifferentValueThan<T, TU>(this ICheck<T> check, TU expected)
        {
            return EqualityHelper.PerformInequalCheck(check, expected, true);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<T>> IsNotEqualTo<T>(this ICheck<T> check, object expected)
        {
            return EqualityHelper.PerformInequalCheck(check, expected);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<T>> IsNotEqualTo<T>(this ICheck<T> check, T expected)
        {
            return IsNotEqualTo(check, (object)expected);
        }

        /// <summary>
        /// Checks that the actual expression is in the inheritance hierarchy of the given kind or of the same kind.
        /// </summary>
        /// <typeparam name="T">The Type which is expected to be a base Type of the actual expression.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <exception cref="FluentCheckException">The checked expression is not in the inheritance hierarchy of the given kind.</exception>
        public static void InheritsFrom<T>(this ICheck<object> check)
        {
            check.InheritsFromType(typeof(T));
        }

        /// <summary>
        /// Checks that the actual expression is in the inheritance hierarchy of the given kind or of the same kind.
        /// </summary>
        /// <typeparam name="T">Type of SUT</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="parentType">Expected type that should be^part of hierarchy</param>
        /// <returns>a check link object</returns>
        public static ICheckLink<ICheck<T>> InheritsFromType<T>(this ICheck<T> check, Type parentType)
        {
            IsInstanceHelper.InheritsFrom(check, parentType);
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the actual expression is null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked value is not null.</exception>
        public static ICheckLink<ICheck<T>> IsNull<T>(this ICheck<T> check) where T : class
        {
            ExtensibilityHelper.BeginCheck(check)
                .SutNameIs("object")
                .FailsIf(sut => sut!= null, "The {0} must be null.")
                .Negates("The {0} must not be null.", MessageOption.NoCheckedBlock)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the actual Nullable value is null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked value is not null.</exception>
        public static ICheckLink<ICheck<T?>> IsNull<T>(this ICheck<T?> check) where T : struct
        {
            ExtensibilityHelper.BeginCheck(check)
                .SutNameIs("nullable value")
                .FailsIf(sut => sut!= null, "The {0} must be null.")
                .Negates("The {0} is null whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the actual Nullable value is not null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked value is null.</exception>
        public static ICheckLink<ICheck<T?>> IsNotNull<T>(this ICheck<T?> check) where T : struct
        {
            return check.Not.IsNull();
        }

        /// <summary>
        /// Checks that the actual expression is not null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">Is the value is null.</exception>
        public static ICheckLink<ICheck<T>> IsNotNull<T>(this ICheck<T> check) where T : class
        {
            return check.Not.IsNull();
        }

        /// <summary>
        /// Obsolete. Use <see cref="ObjectCheckExtensions.IsSameReferenceAs{T, TU}"/> instead. 
        /// Checks that the actual value has an expected reference.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <typeparam name="TU">Type of expected reference</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected object.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not the same reference than the expected value.</exception>
        [Obsolete("Use IsSameReferenceAs")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ICheckLink<ICheck<T>> IsSameReferenceThan<T, TU>(this ICheck<T> check, TU expected)
        {
            return IsSameReferenceAs(check, expected);
        }

        /// <summary>
        /// Checks that the actual value has an expected reference.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <typeparam name="TU">Type of expeted reference</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected object.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not the same reference than the expected value.</exception>
        public static ICheckLink<ICheck<T>> IsSameReferenceAs<T, TU>(this ICheck<T> check, TU expected)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SutNameIs("object")
                .ComparingTo(expected, "same instance as", "distinct from")
                .FailsIf(sut => !ReferenceEquals(sut, expected), "The {0} must be the same instance as the {1}.")
                .Negates("The {0} must be an instance distinct from the {1}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the actual value is a different instance than a comparand.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <typeparam name="TU">Type of reference value.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="comparand">The expected value to be distinct from.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is the same instance than the comparand.</exception>
        public static ICheckLink<ICheck<T>> IsDistinctFrom<T, TU>(this ICheck<T> check, TU comparand)
        {
            return check.Not.IsSameReferenceAs(comparand);
        }
    }
}

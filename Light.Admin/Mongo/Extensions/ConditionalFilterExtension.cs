using MongoDB.Filters;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public static class ConditionalFilterExtension
    {
        public static ConditionalFilter<T> IfNoDefault<T, TValue>(this FilterDefinition<T> filter, TValue value)
            where TValue : IEquatable<TValue>
        {
            return filter.If(Equals(value, default));
        }

        public static ConditionalFilter<T> IfAny<T, TValue>(this SingleConditionalFilter<T> filter, IEnumerable<TValue> values)
        {
            return filter.If(values != null && values.Any());
        }

        public static ConditionalFilter<TModel> If<TModel, TValue>(this FilterDefinition<TModel> filter, TValue? value) where TValue : struct
        {
            return new SingleConditionalFilter<TModel>(filter, () => value.HasValue);
        }

        public static ConditionalFilter<T> If<T>(this FilterDefinition<T> filter, bool condition)
        {
            return new SingleConditionalFilter<T>(filter, () => condition);
        }

        public static ConditionalFilter<T> If<T>(this FilterDefinition<T> filter, bool? condition)
        {
            return new SingleConditionalFilter<T>(filter, () => condition == true);
        }

        public static ConditionalFilter<T> If<T>(this FilterDefinition<T> filter, string? value)
        {
            return filter.If(string.IsNullOrEmpty(value) == false);
        }


        public static ConditionalFilter<T> IfAll<T>(this FilterDefinition<T> filter, params bool[] conditions)
        {
            return new SingleConditionalFilter<T>(filter, () => conditions.All(c => c));
        }

        public static ConditionalFilter<T> IfAny<T>(this FilterDefinition<T> filter, params bool[] conditions)
        {
            return new SingleConditionalFilter<T>(filter, () => conditions.Any(c => c));
        }

        public static ConditionalFilter<T> IfAny<T, TValue>(this FilterDefinition<T> filter, IEnumerable<TValue>? values)
        {
            return filter.If(values != null && values.Any());
        }
    }
}

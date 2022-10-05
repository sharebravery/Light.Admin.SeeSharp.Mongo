using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MongoDB.Filters
{
    abstract public class ConditionalFilter<T>
    {
        abstract public FilterDefinition<T>? Build();

        public static ConditionalFilter<T> operator &(ConditionalFilter<T> left, ConditionalFilter<T> right)
        {
            return new BinaryConditionalFilter<T>(left, right, true);
        }

        public static ConditionalFilter<T> operator |(ConditionalFilter<T> left, ConditionalFilter<T> right)
        {
            return new BinaryConditionalFilter<T>(left, right, false);
        }

        public static ConditionalFilter<T> operator !(ConditionalFilter<T> filter)
        {
            return new NotConditionalFilter<T>(filter);
        }

        public static ConditionalFilter<T> operator &(ConditionalFilter<T> left, FilterDefinition<T> right)
        {
            return new BinaryConditionalFilter<T>(left, new SingleConditionalFilter<T>(right), true);
        }

        public static ConditionalFilter<T> operator |(ConditionalFilter<T> left, FilterDefinition<T> right)
        {
            return new BinaryConditionalFilter<T>(left, new SingleConditionalFilter<T>(right), false);
        }

        public static ConditionalFilter<T> operator &(FilterDefinition<T> left, ConditionalFilter<T> right)
        {
            return new BinaryConditionalFilter<T>(new SingleConditionalFilter<T>(left), right, true);
        }

        public static ConditionalFilter<T> operator |(FilterDefinition<T> left, ConditionalFilter<T> right)
        {
            return new BinaryConditionalFilter<T>(new SingleConditionalFilter<T>(left), right, false);
        }

        static public implicit operator FilterDefinition<T>(ConditionalFilter<T> conditionalFilter)
        {
            return conditionalFilter.Build() ?? Builders<T>.Filter.Empty;
        }

        static public implicit operator ConditionalFilter<T>(FilterDefinition<T> filter)
        {
            return new SingleConditionalFilter<T>(filter);
        }
    }
}

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;

namespace MongoDB.Filters
{
    public class SingleConditionalFilter<T>
        : ConditionalFilter<T>
    {
        static readonly Func<bool> emptyCondition = () => true;

        public SingleConditionalFilter(FilterDefinition<T> filter)
            : this(filter, null)
        {

        }

        public SingleConditionalFilter(FilterDefinition<T> filter, Func<bool>? condition)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            Filter = filter;
            Condition = condition ?? emptyCondition;
        }

        public SingleConditionalFilter<T> If(bool condition)
        {
            return If(() => condition);
        }

        public SingleConditionalFilter<T> If(Func<bool> condition)
        {
            Condition = condition;
            return this;
        }

        public override FilterDefinition<T>? Build()
        {
            if (Condition == null ||
                Condition() == true)
            {
                return Filter;
            }
            return null;
        }

        public FilterDefinition<T> Filter { get; set; }

        public Func<bool> Condition { get; set; }


        //public static implicit operator SingleConditionalFilter<T>(FilterDefinition<T> filter)
        //{
        //    return new SingleConditionalFilter<T>(filter);
        //}
    }

}

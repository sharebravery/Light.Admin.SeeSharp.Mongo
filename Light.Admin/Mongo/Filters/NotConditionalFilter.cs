using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Filters
{
    public class NotConditionalFilter<T>
        : ConditionalFilter<T>
    {
        public NotConditionalFilter(ConditionalFilter<T> inner)
        {
            Inner = inner;
        }

        public ConditionalFilter<T> Inner { get; }

        public override FilterDefinition<T>? Build()
        {
            var inenr = Inner.Build();
            if (inenr != null)
            {
                return Builders<T>.Filter.Not(inenr);
            }
            return null;
        }
    }
}

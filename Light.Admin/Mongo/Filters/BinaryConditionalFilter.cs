using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MongoDB.Filters
{

    public class BinaryConditionalFilter<T>
        : ConditionalFilter<T>
    {
        public BinaryConditionalFilter(
            ConditionalFilter<T> left,
            ConditionalFilter<T> right,
            bool isAndAlso = true)
        {
            Left = left;
            Right = right;
            IsAndAlso = isAndAlso;
        }

        public bool IsAndAlso { get; }
        public ConditionalFilter<T> Left { get; }
        public ConditionalFilter<T> Right { get; }

        public override FilterDefinition<T>? Build()
        {
            var left = Left?.Build();
            var right = Right?.Build();
            if (left == null)
            {
                return right;
            }
            if (right == null)
            {
                return left;
            }
            if(IsAndAlso)
            {
                return Builders<T>.Filter.And(left, right);
            }
            return Builders<T>.Filter.Or(left, right);
        }
    }
}

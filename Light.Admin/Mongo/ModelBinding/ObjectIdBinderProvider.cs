using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    public class ObjectIdBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(ObjectId) || context.Metadata.ModelType == typeof(ObjectId?))
            {
                return ObjectIdModelBinder;
            }

            return null;
        }

        static public IModelBinder ObjectIdModelBinder { get; } = new BinderTypeModelBinder(typeof(ObjectIdBinder));
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.ModelBinding.Metadata
{
    public class ObjectIdBindingMetadataProvider : IBindingMetadataProvider, IMetadataDetailsProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if ((context.Key.ModelType == typeof(ObjectId) || context.Key.ModelType == typeof(ObjectId?))
                && context.Key.ContainerType == null)
            {
                if(context.BindingMetadata.BindingSource == null)
                {
                    context.BindingMetadata.BindingSource = BindingSource.Custom;
                }
            }
        }
    }
}

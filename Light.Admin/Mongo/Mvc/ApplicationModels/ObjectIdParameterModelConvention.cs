using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.ApplicationModels
{
    public class ObjectIdParameterModelConvention : IActionModelConvention
    {
        //public void Apply(ParameterModel parameter)
        //{
        //    if(parameter.ParameterType == typeof(ObjectId) || parameter.ParameterType == typeof(ObjectId?))
        //    {
        //        if (parameter.BindingInfo.BindingSource == BindingSource.Body
        //            && !parameter.Attributes.OfType<FromBodyAttribute>().Any())
        //        {
        //            parameter.BindingInfo.BindingSource = BindingSource.Query;
        //        }
        //    }
        //}

        public void Apply(ActionModel action)
        {
            foreach (var parameter in action.Parameters.Where(p => (p.ParameterType == typeof(ObjectId) || p.ParameterType == typeof(ObjectId?))
                && p.BindingInfo?.BindingSource == BindingSource.Custom))
            {

            }
        }
    }


}

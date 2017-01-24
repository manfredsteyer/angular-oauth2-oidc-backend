using Demo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.OData;

namespace WebApi_Demo
{
    public class CustomEnableQuery: EnableQueryAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var result = new ODataMetadata();

            var queryString = actionExecutedContext.Request.GetQueryNameValuePairs();
            var countRequested = queryString.Any(q => q.Key == "$count" && q.Value == "true");
            
            if (countRequested) {

                var contentBefore = actionExecutedContext.Response.Content as ObjectContent;
                if (contentBefore == null) return;

                var queryBefore = contentBefore.Value as IQueryable<object>;
                if (queryBefore == null) return;

                result.Count = queryBefore.Count();

            }

            base.OnActionExecuted(actionExecutedContext);

            if (!actionExecutedContext.Response.IsSuccessStatusCode) return;

            var status = actionExecutedContext.Response.StatusCode;
            var contentAfter = actionExecutedContext.Response.Content as ObjectContent;
            if (contentAfter == null) return;

            var queryAfter = contentAfter.Value as IEnumerable<object>;
            if (queryAfter == null) return;

            result.Value = queryAfter;

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(status, result );
        }
    }
}
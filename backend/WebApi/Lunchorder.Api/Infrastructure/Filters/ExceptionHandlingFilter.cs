﻿using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Elmah;
using Lunchorder.Domain.Exceptions;

namespace Lunchorder.Api.Infrastructure.Filters
{
    /// <summary>
    /// Filter that catches all the exceptions that happen on the platform
    /// </summary>
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is BusinessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(HttpUtility.HtmlEncode(context.Exception.Message))
                });
            }
            if (context.Exception is UnauthorizedAccessException || context.Exception is SecurityException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            //Log Critical errors
            if (HttpContext.Current == null)
            {
                ErrorLog.GetDefault(null).Log(new Elmah.Error(context.Exception));
            }
            else
            {
                ErrorSignal.FromCurrentContext().Raise(context.Exception, HttpContext.Current);
            }

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Something went wrong, please contact your administrator")
            });
        }
    }
}
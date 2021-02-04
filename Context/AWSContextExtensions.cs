using System;
using AusDdrApi.Authentication;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Context
{
    public static class AWSContextExtensions
    {
        public static AWSConfiguration GetAWSConfiguration(this HttpContext context)
        {
            return (AWSConfiguration)context.Items["AWSConfiguration"];
        }
    }
}
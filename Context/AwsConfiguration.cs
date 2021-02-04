using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;

namespace AusDdrApi.Context
{
    public class AwsConfiguration
    {
        public string AssetsBucketName { get; set; }
        public string AssetsBucketLocation { get; set; }
    }
}
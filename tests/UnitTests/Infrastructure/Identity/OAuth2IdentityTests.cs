using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Core.Entities;
using Infrastructure.Cache;
using Infrastructure.Identity;
using Xunit;

namespace UnitTests.Infrastructure.Identity
{
    public class OAuth2IdentityTests
    {
        [Fact(DisplayName = "When jwt contains cognito:groups claim including value administrators, returns true")]
        public void JwtContainingGroupsAndAdministrators()
        {
            var config = new OAuth2IdentityConfig
            {
                
            };
            var oauth2 = new OAuth2Identity(new InMemoryCache(), config, new HttpClient());

            var jwtWithAdministrators =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJteS1pc3MiLCJpYXQiOjE1Nzc4MzY4MDAsImV4cCI6NDEwMjQ0NDgwMCwiYXVkIjoid3d3Lm15LWF1ZC5jb20iLCJzdWIiOiJzdWIiLCJjb2duaXRvOmdyb3VwcyI6WyJhZG1pbmlzdHJhdG9ycyIsImdyb3VwMiJdfQ.4LD_dq4ePCMBdMylRdneDx7Kp6xkWLwmF3yBtB17Zmw";

            var result = oauth2.IsAdmin(jwtWithAdministrators);

            Assert.True(result);
        }
        
        [Fact(DisplayName = "When jwt contains cognito:groups claim but does not contain administrators, returns false")]
        public void JwtContainingGroupsButNoAdministrators()
        {
            var config = new OAuth2IdentityConfig
            {
                
            };
            var oauth2 = new OAuth2Identity(new InMemoryCache(), config, new HttpClient());

            var jwtWithoutAdministrators =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJteS1pc3MiLCJpYXQiOjE1Nzc4MzY4MDAsImV4cCI6NDEwMjQ0NDgwMCwiYXVkIjoid3d3Lm15LWF1ZC5jb20iLCJzdWIiOiJzdWIiLCJjb2duaXRvOmdyb3VwcyI6WyJncm91cDEiLCJncm91cDIiXX0.rD9rWpxZ-CYddcmMDgHKKTRK6IPHgkP5r_heplBKeAs";

            var result = oauth2.IsAdmin(jwtWithoutAdministrators);

            Assert.False(result);
        }
        
        [Fact(DisplayName = "When jwt does not contain cognito:groups claim, returns false")]
        public void JwtWithoutGroupsClaim()
        {
            var config = new OAuth2IdentityConfig
            {
                
            };
            var oauth2 = new OAuth2Identity(new InMemoryCache(), config, new HttpClient());

            var jwtWithoutGroupsClaim =
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJteS1pc3MiLCJpYXQiOjE1Nzc4MzY4MDAsImV4cCI6NDEwMjQ0NDgwMCwiYXVkIjoid3d3Lm15LWF1ZC5jb20iLCJzdWIiOiJzdWIifQ.dSm_NuJx9eH6wosTPU5CNK5EV7o0LiWLZ2tYpaC9IHA";

            var result = oauth2.IsAdmin(jwtWithoutGroupsClaim);

            Assert.False(result);
        }

        [Fact]
        public async Task Test()
        {
            var repository = InMemoryDatabaseRepository<Dancer>.CreateRepository();

            await repository.AddAsync(new Dancer()
            {
                AuthenticationId = "test"
            });
            await repository.SaveChangesAsync();

            var dancer = await repository.ListAsync();
            Console.WriteLine("Hello");
        }
    }
}
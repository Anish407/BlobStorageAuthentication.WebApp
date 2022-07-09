using Azure.Core;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Linq;

namespace BlobStorageAuthentication.WebApp.Auth
{
    public class UserAcquisitionTokenCredential : TokenCredential
    {
        public UserAcquisitionTokenCredential(ITokenAcquisition tokenAcquisition,
        IConfiguration configuration)
        {
            TokenAcquisition = tokenAcquisition;
            Configuration = configuration;
        }

        public ITokenAcquisition TokenAcquisition { get; }
        public IConfiguration Configuration { get; }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [AuthorizeForScopes(Scopes = new[] { "https://storage.azure.com/.default" })]
        //https://docs.microsoft.com/en-us/dotnet/api/microsoft.identity.client.iaccount?view=azure-dotnet&preserve-view=true
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            // requestContext.Scopes "https://storage.azure.com/.default"
            string[] scopes = new string[] { "https://storage.azure.com/.default" };
            string clientId = Configuration["AzureAd:ClientId"];
            string tenantId = Configuration["AzureAd:TenantId"];
            string secret = Configuration["AzureAd:ClientSecret"];
            //IPublicClientApplication app = PublicClientApplicationBuilder
            // .Create(clientId)
            // .WithAuthority(tenantId)
            // .Build();

            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(secret)
                .WithAuthority("https://login.microsoftonline.com/06e2775e-9d3d-49de-ad36-da82e295fa67/")
                .WithTenantId(tenantId)
                .WithRedirectUri("http://localhost")
                .Build();
            //IPublicClientApplication app = PublicClientApplicationBuilder
            //   .Create(clientId)
            //   .WithAuthority("https://login.microsoftonline.com/06e2775e-9d3d-49de-ad36-da82e295fa67/")
            //   .WithTenantId(tenantId)
            //   .WithRedirectUri("http://localhost")
            //   .Build();
            try
            {
                //var accounts = await app.GetAccountsAsync();

                //if(accounts.FirstOrDefault() != null)
                //{
                //var result2 = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                //return new AccessToken(result2.AccessToken, result2.ExpiresOn);
                //}
                //AuthenticationResult result = await TokenAcquisition
                //   .GetAuthenticationResultForUserAsync(scopes)
                //   .ConfigureAwait(false);

                //  return new AccessToken(result.AccessToken, result.ExpiresOn);
                //var token = await TokenAcquisition.GetAccessTokenForUserAsync(scopes);
                //var result = await app.(scopes).ExecuteAsync();
                //return new AccessToken(result.AccessToken, result.ExpiresOn);
                //var accounts = await app.GetAccountsAsync();

                ////var result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                ////string token = await TokenAcquisition.GetAccessTokenForUserAsync(scopes);
                ////AuthenticationResult result = await TokenAcquisition
                ////       .GetAuthenticationResultForUserAsync(scopes);
                //return new AccessToken(token, DateTime.Now.AddHours(2));
                var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                return new AccessToken(result.AccessToken, result.ExpiresOn);
            }
            catch (MsalUiRequiredException ex)
            {
                //IPublicClientApplication app = PublicClientApplicationBuilder
                //   .Create(clientId)
                //   .WithAuthority("https://login.microsoftonline.com/06e2775e-9d3d-49de-ad36-da82e295fa67/")
                //   .WithTenantId(tenantId)
                //   .WithRedirectUri("http://localhost")
                //   .Build();
                throw;
                //var accounts = await app.GetAccountsAsync();

                //var result = await app.AcquireTokenForClient(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                //return new AccessToken(result.AccessToken, result.ExpiresOn);
            }
            catch (Exception ex)
            {
                try
                {
                    // IPublicClientApplication app = PublicClientApplicationBuilder
                    //.Create(clientId)
                    //.WithAuthority("https://login.microsoftonline.com/06e2775e-9d3d-49de-ad36-da82e295fa67/")
                    //.WithTenantId(tenantId)
                    //.WithRedirectUri("http://localhost")
                    //.Build();

                    var accounts = await app.GetAccountsAsync();

                    var result = await app.AcquireTokenSilent(scopes,accounts.FirstOrDefault()).ExecuteAsync();
                    return new AccessToken(result.AccessToken, result.ExpiresOn);
                }
                catch (Exception ex1)
                {

                    throw;
                }
            }


        }
    }
}

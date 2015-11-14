using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Soloco.ReactiveStarterKit.Membership.Messages.ViewModel;

namespace Soloco.ReactiveStarterKit.Membership.Domain
{
    public class ExternalLoginData
    {
        public LoginProvider LoginProvider { get; private set; }
        public string ProviderKey { get; private set; }
        public string UserName { get; private set; }
        public string ExternalAccessToken { get; private set; }

        public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        {
            var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer.AsLoginProvider(),
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirstValue(ClaimTypes.Name),
                ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
            };
        }

    }
}
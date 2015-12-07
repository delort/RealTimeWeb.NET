using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNet.Identity;
using Soloco.RealTimeWeb.Common.Infrastructure;
using Soloco.RealTimeWeb.Common.Infrastructure.Messages;
using Soloco.RealTimeWeb.Membership.Domain;
using Soloco.RealTimeWeb.Membership.Messages.Commands;
using Soloco.RealTimeWeb.Membership.Messages.Queries;
using Soloco.RealTimeWeb.Membership.Services;

namespace Soloco.RealTimeWeb.Membership.CommandHandlers
{
    public class RegisterExternalUserHandler : CommandHandler<RegisterExternalUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IProviderTokenValidatorFactory _providerTokenValidatorFactory;

        public RegisterExternalUserHandler(IDocumentSession session, IDisposable scope, IProviderTokenValidatorFactory providerTokenValidatorFactory) : base(session, scope)
        {
            _providerTokenValidatorFactory = providerTokenValidatorFactory;

            var userStore = new UserStore(session);
            _userManager = new UserManager<User>(userStore, null,null, null, null, null, null, null, null, null);
        }

        protected override async Task<CommandResult> Execute(RegisterExternalUserCommand command)
        {
            var validator = _providerTokenValidatorFactory.Create(command.Provider);
            var verifiedAccessToken = await validator.ValidateToken(command.ExternalAccessToken);

            await VerifyNotRegistered(command, verifiedAccessToken);

            var user = await CreateUser(command);

            return await CreateLogin(command, user, verifiedAccessToken.UserId, command.UserName);
        }

        private async Task VerifyNotRegistered(RegisterExternalUserCommand command, ParsedExternalAccessToken verifiedAccessToken)
        {
            var query = new UserLoginQuery(command.Provider, verifiedAccessToken.UserId);
            var userLogin = await _userManager.FindByLoginAsync(query.LoginProvider.ToString(), query.ProviderKey);

            if (userLogin != null)
            {
                throw new BusinessException("External user is already registered");
            }
        }

        private async Task<User> CreateUser(RegisterExternalUserCommand command)
        {
            var user = new User(command.UserName);
            var result = await _userManager.CreateAsync(user);

            result.ThrowWhenFailed("Could not create user");

            return user;
        }

        private async Task<CommandResult> CreateLogin(RegisterExternalUserCommand command, User user, string verifiedAccessTokenuser_id, string displayName)
        {
            var login = new UserLoginInfo(command.Provider.ToString(), verifiedAccessTokenuser_id, displayName);

            var result = await _userManager.AddLoginAsync(user, login);
            return result.ToCommandResult();
        }
    }
}
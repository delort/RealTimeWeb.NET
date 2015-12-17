using System;
using System.Threading.Tasks;
using Marten;
using Soloco.RealTimeWeb.Common;
using Soloco.RealTimeWeb.Common.Messages;
using Soloco.RealTimeWeb.Membership.Domain;
using Soloco.RealTimeWeb.Membership.Messages.Commands;

namespace Soloco.RealTimeWeb.Membership.CommandHandlers
{
    public class DeleteRefreshTokenHandler : CommandHandler<DeleteRefreshTokenCommand>
    {
        public DeleteRefreshTokenHandler(IDocumentSession session) : base(session)
        {
        }

        protected override Task<CommandResult> Execute(DeleteRefreshTokenCommand command)
        {
            return Task.FromResult(GetResult(command));
        }

        private CommandResult GetResult(DeleteRefreshTokenCommand command)
        {
            var token = Session.Load<RefreshToken>(command.TokenId);
            if (token == null)
            {
                return CommandResult.Failed("Could not find token");
            }

            Session.Delete(token);
            return CommandResult.Success;
        }
    }
}
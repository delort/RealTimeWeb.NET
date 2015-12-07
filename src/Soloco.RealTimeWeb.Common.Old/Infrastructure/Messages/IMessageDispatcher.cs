﻿using System.Threading.Tasks;

namespace Soloco.RealTimeWeb.Common.Infrastructure.Messages
{
    public interface IMessageDispatcher
    {
        Task<TResult> Execute<TResult>(IMessage<TResult> message); 
    }
}
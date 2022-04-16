using System;
using AuthServer.Core.Dtos;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class UserQuery : IRequest<UserDetailInfoDto>
    {
        public Guid Id { get; set; }
    }
}
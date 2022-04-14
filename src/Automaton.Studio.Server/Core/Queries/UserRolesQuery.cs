using System;
using System.Collections.Generic;
using MediatR;

namespace AuthServer.Core.Queries
{
    public class UserRolesQuery : IRequest<IEnumerable<string>>
    {
        public Guid UserId { get; set; }
    }
}
using MediatR;

namespace E_Wallet.Application.Contracts.Queries;

public sealed class GetUserByIdQuery : IRequest<DataResult<UserDto>>
{
    public string? Id { get; set; }
}

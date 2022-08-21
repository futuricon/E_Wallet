namespace E_Wallet.Application.Handlers.Queries;

internal sealed class GetUserByIdHandler : RequestHandlerBase<GetUserByIdQuery, DataResult<UserDto>>
{
    private readonly IRepository<User> _userRepository;

    public GetUserByIdHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<DataResult<UserDto>> HandleValidated(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetAsync(i => i.Id == request.Id);

        if (userEntity == null)
            return DataResult<UserDto>.CreateError("Could not find the specified user");

        var user = new UserDto
        {
            Id = userEntity.Id,
            UserName = userEntity.UserName,
            APIKey = userEntity.APIKey,
            AppId = userEntity.AppId,
            IsIdentified = userEntity.IsIdentified,
            WalletId = userEntity.WalletId
        };

        return DataResult<UserDto>.CreateSuccess(user);
    }

    protected override bool Validate(GetUserByIdQuery request, out string errorDescription)
    {
        errorDescription = string.Empty;

        if (string.IsNullOrEmpty(request.Id))
        {
            errorDescription = "Id is an empty string";
        }

        return string.IsNullOrEmpty(errorDescription);
    }
}

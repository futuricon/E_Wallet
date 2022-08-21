namespace E_Wallet.Application.Handlers.Queries;

internal sealed class GetWalletBalanceByIdHandler : RequestHandlerBase<GetWalletBalanceByIdQuery, DataResult<decimal>>
{
    private readonly IRepository<Wallet> _walletRepository;

    public GetWalletBalanceByIdHandler(IRepository<Wallet> walletRepository)
    {
        _walletRepository = walletRepository;
    }

    protected override async Task<DataResult<decimal>> HandleValidated(GetWalletBalanceByIdQuery request, CancellationToken cancellationToken)
    {
        var walletEntity = await _walletRepository.GetAsync(i => i.Id == request.Id);

        if (walletEntity == null)
            return DataResult<decimal>.CreateError("Could not find the specified wallet");


        return DataResult<decimal>.CreateSuccess(walletEntity.Balance);
    }

    protected override bool Validate(GetWalletBalanceByIdQuery request, out string errorDescription)
    {
        errorDescription = string.Empty;

        if (string.IsNullOrEmpty(request.Id))
        {
            errorDescription = "Id is an empty string";
        }

        return string.IsNullOrEmpty(errorDescription);
    }
}


namespace E_Wallet.Application.Handlers.Commands;

internal sealed class UpdateWalletHandler : RequestHandlerBase<UpdateWalletCommand, DataResult>
{
    private readonly IRepository<Wallet> _walletRepository;

    public UpdateWalletHandler(IRepository<Wallet> walletRepository)
    {
        _walletRepository = walletRepository;
    }

    protected override async Task<DataResult> HandleValidated(
        UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(request.Id!);

        if (wallet == null)
            return DataResult.CreateError("Could not find the specified wallet");

        wallet.Balance = request.Balance;
        wallet.LastModifiedDate = DateTime.UtcNow;

        _walletRepository.Update(wallet);
        await _walletRepository.UnitOfWork.CommitAsync();
        return DataResult.CreateSuccess();
    }

    protected override bool Validate(UpdateWalletCommand request, out string errorDescription)
    {
        errorDescription = string.Empty;
        
        if (string.IsNullOrEmpty(request.Id))
        {
            errorDescription = "WalletId is an empty string";
        }
        else if (request.Balance < 0)
        {
            errorDescription = "Amount should be non-negative value";
        }

        return string.IsNullOrEmpty(errorDescription);
    }
}
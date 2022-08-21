using E_Wallet.Domain.Shared;

namespace E_Wallet.Application.Handlers.Commands;

internal sealed class CreateTransactionHandler : RequestHandlerBase<CreateTransactionCommand, DataResult<decimal>>
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Wallet> _walletRepository;

    public CreateTransactionHandler(
        IRepository<Transaction> transactionRepository,
        IRepository<User> userRepository,
        IRepository<Wallet> walletRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
    }

    protected override async Task<DataResult<decimal>> HandleValidated(
        CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        //var user = await _userRepository.GetAsync(request.UserId!);

        //if (user == null)
            //return DataResult<decimal>.CreateError("Could not find the specified user");

        var wallet = await _walletRepository.GetAsync(request.WalletId!);

        if (wallet == null)
            return DataResult<decimal>.CreateError("Could not find the specified wallet");

        var transactionEntity = new Transaction
        {
            //User = user,
            Wallet = wallet,
            Amount = request.Amount,
            TransactionTypeId = request.TransactionTypeId
        };

        var currentBalance = wallet.Balance + transactionEntity.Amount;

        //gotta get it out of here
        //if (transactionEntity.TransactionTypeId == (int)TransactionTypes.Replanish && 
        //    ((currentBalance > 100000m && user.IsIdentified == true) || 
        //    (currentBalance > 10000m && user.IsIdentified == false)))
        //{
        //    return DataResult<decimal>.CreateError("Balance limit exceeded");
        //}
        //else if (transactionEntity.TransactionTypeId == (int)TransactionTypes.Withdraw && 
        //    (wallet.Balance - transactionEntity.Amount) < 0m)
        //{
        //    return DataResult<decimal>.CreateError("Insufficient funds on the balance");
        //}

        await _transactionRepository.AddAsync(transactionEntity);
        await _transactionRepository.UnitOfWork.CommitAsync();
        return DataResult<decimal>.CreateSuccess(currentBalance);
    }

    protected override bool Validate(CreateTransactionCommand request, out string errorDescription)
    {
        errorDescription = string.Empty;

        //if (string.IsNullOrEmpty(request.UserId))
        //{
        //    errorDescription = "UserId is an empty string";
        //}
        if (string.IsNullOrEmpty(request.WalletId))
        {
            errorDescription = "WalletId is an empty string";
        }
        else if (request.Amount < 0)
        {
            errorDescription = "Amount should be non-negative value";
        }
        else if (!Enum.IsDefined(typeof(TransactionTypes), request.TransactionTypeId))
        {
            errorDescription = "Provide valid TransactionTypeId";
        }

        return string.IsNullOrEmpty(errorDescription);
    }
}

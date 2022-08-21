namespace E_Wallet.Application.Handlers.Queries;

public class GetTransactionsHandler : RequestHandlerBase<GetTransactionsQuery, DataResult<MonthlyTransactionsDto>>
{
    private readonly IRepository<Transaction> _transactionRepository;

    public GetTransactionsHandler(
        IRepository<Transaction> transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    protected override Task<DataResult<MonthlyTransactionsDto>> HandleValidated(
        GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactionsQuery = _transactionRepository.GetQuery();

        if (!string.IsNullOrEmpty(request.UserId) && request.TransactionTypeId != null && request.Period != null)
        {
            transactionsQuery = _transactionRepository.GetQuery(x=> x.UserId == request.UserId 
            && x.TransactionDate.Year == request.Period.Value.Year 
            && x.TransactionDate.Month == request.Period.Value.Month
            && x.TransactionTypeId == request.TransactionTypeId);
        }

        var transactions = transactionsQuery
            .OrderBy(i => i.TransactionDate)
            .ToArray();

        var monthlyTransactionsDto = new MonthlyTransactionsDto() {
            Quantity = transactions.Count(),
            Amount = transactions.Select(i=>i.Amount).Sum(),
        };

        return Task.FromResult(DataResult<MonthlyTransactionsDto>.CreateSuccess(monthlyTransactionsDto));
    }

    protected override bool Validate(GetTransactionsQuery request, out string errorDescription)
    {
        errorDescription = string.Empty;

        if (request.Period == null)
        {
            errorDescription = "Specify Period";
        }
        else if (request.TransactionTypeId == null)
        {
            errorDescription = "Specify the type of Transaction";
        }
        else if (request.UserId == null)
        {
            errorDescription = "UserId is null";
        }

        return string.IsNullOrEmpty(errorDescription);
    }
}


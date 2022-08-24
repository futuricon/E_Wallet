using MediatR;

namespace E_Wallet.Application.Contracts.Queries;

public class GetTransactionsQuery : BaseQuery, IRequest<DataResult<MonthlyTransactionsDto>>
{
    public int? TransactionTypeId { get; set; }
    public DateTime? Period { get; set; }
}

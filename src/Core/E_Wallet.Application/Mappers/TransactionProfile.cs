namespace E_Wallet.Application.Mappers;

internal class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, CreateTransactionCommand>().ReverseMap();
        CreateMap<TransactionDto, CreateTransactionCommand>().ReverseMap();
    }
}
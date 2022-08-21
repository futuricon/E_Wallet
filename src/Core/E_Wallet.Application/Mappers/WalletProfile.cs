namespace E_Wallet.Application.Mappers;

internal class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, CreateWalletCommand>().ReverseMap();
        CreateMap<WalletDto, CreateWalletCommand>().ReverseMap();
    }
}

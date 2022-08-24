namespace E_Wallet.Application.Mappers;

internal class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, CreateWalletCommand>().ReverseMap();
        CreateMap<Wallet, UpdateWalletCommand>().ReverseMap();
        CreateMap<WalletDto, CreateWalletCommand>().ReverseMap();
        CreateMap<WalletDto, UpdateWalletCommand>().ReverseMap();
    }
}

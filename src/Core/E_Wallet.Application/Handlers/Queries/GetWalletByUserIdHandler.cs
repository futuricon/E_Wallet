namespace E_Wallet.Application.Handlers.Queries;

internal sealed class GetWalletByUserIdHandler : RequestHandlerBase<GetWalletByUserIdQuery, DataResult<WalletDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Wallet> _walletRepository;

        public GetWalletByUserIdHandler(
            IRepository<User> userRepository,
            IRepository<Wallet> walletRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

        protected override async Task<DataResult<WalletDto>> HandleValidated(GetWalletByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetAsync(i => i.Id == request.UserId);
            
            if (userEntity == null)
                return DataResult<WalletDto>.CreateError("Could not find the specified user");
            
            var walletEntity = await _walletRepository.GetAsync(i => i.UserId == userEntity.Id);
            
            if (walletEntity == null)
                return DataResult<WalletDto>.CreateError("the user does not have an e-wallet");
            
            var wallet = new WalletDto
            {
                Id = walletEntity.Id,
                Balance = walletEntity.Balance,
                UserId = walletEntity.UserId,
                LastModifiedDate = walletEntity.LastModifiedDate,
            };

            return DataResult<WalletDto>.CreateSuccess(wallet);
        }

        protected override bool Validate(GetWalletByUserIdQuery request, out string errorDescription)
        {
            errorDescription = string.Empty;

            if (string.IsNullOrEmpty(request.UserId))
            {
                errorDescription = "UserId is an empty string";
            }

            return string.IsNullOrEmpty(errorDescription);
        }
    }

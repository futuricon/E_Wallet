using E_Wallet.Application.Contracts.Commands;
using E_Wallet.Application.Contracts.Queries;
using System.Net.Http.Json;

namespace E_Wallet.SimpleClientApp;

class Program
{
    static void Main(string[] args)
    {
        string command;
        while (true)
        {
            GetKey();
            Console.WriteLine("\nPress  'q'  to quit or any key to continue");
            command = Console.ReadLine();
            if (command == "q" || command == "Q")
            {
                break;
            }            
        }
    }

    static void GetKey()
    {
        Console.WriteLine("\n\n" +
            "\n1.Get the total number and amount of recharge operations for the current month. should get" +
            "\n2.Get the e-wallet balance. should get" +
            "\n3.Check if the e-wallet account exists. should exist" +
            "\n4.Replenish e-wallet account. should replanish" +
            "\n5.Withdraw e-wallet accounts. shouldn`t withdraw" +
            "\n6.Replenish e-wallet accounts. shouldn`t replenish" +
            "\n7.Check if the e-wallet account exists. shouldn`t exist");
        int key = new int();
        Console.WriteLine("\n\nEnter key");
        bool success = int.TryParse(Console.ReadLine(), out key);
        if (!success)
            GetKey();

        switch (key)
        {
            case 1:
                HandleAsync("1aa33b0d-9e93-4cca-b807-37466f76ded1",
                    "transaction/monthlytansactions",
                    new GetTransactionsQuery { TransactionTypeId = 1, Period = DateTime.UtcNow.AddMonths(-1) })
                    .Wait();
                break;
            case 2:
                HandleAsync("266f4157-d3c3-4b78-9c50-0facf05b19b2",
                    "wallet/balance",
                    new GetWalletBalanceByIdQuery { Id = "2w6f4157-d3c3-4b78-9c50-0facf05b19b2" })
                    .Wait();
                break;
            case 3:
                HandleAsync("35c840a0-62d6-462c-8178-571909e01963",
                    "wallet/walletExists",
                    new GetWalletByUserIdQuery { })
                    .Wait();
                break;
            case 4:
                HandleAsync("266f4157-d3c3-4b78-9c50-0facf05b19b2",
                    "transaction/replenish",
                    new CreateTransactionCommand { WalletId = "2w6f4157-d3c3-4b78-9c50-0facf05b19b2", TransactionTypeId = 1, Amount = 1000 })
                    .Wait();
                break;
            case 5:
                HandleAsync("4314de9a-6bc9-4101-af31-43f13a233304",
                    "transaction/replenish",
                    new CreateTransactionCommand { WalletId = "4w14de9a-6bc9-4101-af31-43f13a233304", TransactionTypeId = 2, Amount = 1000 })
                    .Wait();
                break;
            case 6:
                HandleAsync("266f4157-d3c3-4b78-9c50-0facf05b19b2",
                    "transaction/replenish",
                    new CreateTransactionCommand { WalletId = "2w6f4157-d3c3-4b78-9c50-0facf05b19b2", TransactionTypeId = 1, Amount = 10000 })
                    .Wait();
                break;
            case 7:
                HandleAsync("5e93bcf3-7e5d-4f39-b68b-fd56021d2425",
                    "wallet/walletExists",
                    new GetWalletByUserIdQuery { })
                    .Wait();
                break;
        }
    }

    static async Task HandleAsync<T>(string userId, string uri, T hendler)
    {
        Console.WriteLine("Calling the back-end API");

        //provide the port number where your api is running
        string apiBaseAddress = "http://localhost:5030/";

        var customDelegatingHandler = new HMACDelegatingHandler(userId);
        var client = HttpClientFactory.Create(customDelegatingHandler);

        var response = await client.PostAsJsonAsync(apiBaseAddress + uri, hendler);
        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);
        }
        else
        {
            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
        }
    }
}
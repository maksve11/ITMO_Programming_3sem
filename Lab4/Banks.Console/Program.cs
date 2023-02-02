using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Banks.Entities.Banks;
using Banks.Entities.Client;

namespace Banks.Console;

internal static class Program
{
    private static void Main()
    {
        var service = new CentralBank();
        string? line = string.Empty;
        string? name = string.Empty;
        string? surname = string.Empty;
        string? bank = string.Empty;
        string? account = string.Empty;
        string? adress = string.Empty;
        int passport = 0;
        decimal money = 0;
        decimal debitInterest = 0;
        decimal creditInterest = 0;
        decimal depositInterest = 0;
        int amount = 0;
        decimal comission = 0;
        int limit = 0;

        while (line != "8")
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Welcome to bank system!");
            System.Console.WriteLine("------------------------------------------------");
            System.Console.WriteLine("Please choose an action: ");
            System.Console.WriteLine("1 - create new Bank");
            System.Console.WriteLine("2 - add new client");
            System.Console.WriteLine("3 - add an account to a client");
            System.Console.WriteLine("4 - make transaction");
            System.Console.WriteLine("5 - show available banks");
            System.Console.WriteLine("6 - show clients in the bank");
            System.Console.WriteLine("7 - show client account balance");
            System.Console.WriteLine("8 - exit");
            System.Console.Write(">  ");
            line = System.Console.ReadLine();

            switch (line)
            {
                case "1":
                    System.Console.WriteLine("Please enter bank name: ");
                    name = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter debit interest:");
                    debitInterest = Convert.ToDecimal(System.Console.ReadLine());
                    System.Console.WriteLine("Please enter deposit interests:");

                    for (int i = 0; i < 3; i++)
                    {
                        System.Console.WriteLine("Enter amount of money:");
                        amount = Convert.ToInt32(System.Console.ReadLine());
                        System.Console.WriteLine("Enter interest for this amount");
                        debitInterest = Convert.ToDecimal(System.Console.ReadLine());
                        depositInterest = Convert.ToDecimal(System.Console.ReadLine());
                    }

                    System.Console.WriteLine("Please enter credit comission");
                    comission = Convert.ToDecimal(System.Console.ReadLine());
                    System.Console.WriteLine("Please enter credit limit");
                    limit = Convert.ToInt32(System.Console.ReadLine());
                    System.Console.WriteLine("Creating bank .....");
                    System.Console.WriteLine(service.RegisterBank(name, debitInterest, depositInterest, creditInterest, comission, limit).ToString());
                    break;

                case "2":
                    if (service.Banks == null) System.Console.WriteLine("There are no available banks :(");
                    System.Console.WriteLine("Please enter name:");
                    name = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter surname");
                    surname = System.Console.ReadLine();
                    System.Console.WriteLine("Do you want to add adress and passport id? y/n");

                    if (System.Console.ReadLine() == "y")
                    {
                        System.Console.WriteLine("Please enter adress: ");
                        adress = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter passport id: ");
                        passport = Convert.ToInt32(System.Console.ReadLine());
                    }

                    System.Console.WriteLine("Which bank do you want to choose?");
                    System.Console.WriteLine("Available banks:");
                    if (service.Banks != null)
                    {
                        foreach (Bank b in service.Banks)
                        {
                            System.Console.WriteLine($"{b.Name}");
                        }

                        System.Console.WriteLine("Please enter bank name");
                        bank = System.Console.ReadLine();
                        System.Console.WriteLine("Creating new client....");
                        var client = new Client();

                        foreach (Bank b in service.Banks)
                        {
                            if (bank == b.Name)
                            {
                                client = service.AddClientToBank(new ClientBuilder().SetNameAndSurname(name, surname).SetAddress(adress).SetPassport(passport).Build(), b.Id);
                            }
                        }

                        System.Console.WriteLine(client.ToString());
                    }

                    break;

                case "3":
                    if (service.Banks == null) System.Console.WriteLine("There are no available banks and clients :(");
                    System.Console.WriteLine("Please enter account type: ");
                    System.Console.WriteLine("Debit, Deposit, Credit");
                    account = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter bank name:");
                    System.Console.WriteLine("Available banks:");
                    if (service.Banks != null)
                    {
                        foreach (var b in service.Banks)
                        {
                            System.Console.WriteLine($"{b.Name}");
                        }

                        bank = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter client name:");
                        name = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter client surname:");
                        surname = System.Console.ReadLine();

                        System.Console.WriteLine("Please enter account balance:");
                        money = Convert.ToDecimal(System.Console.ReadLine());
                        System.Console.WriteLine("Creating account.....");
                        var bank2 = service.Banks.FirstOrDefault(b => b.Name == bank);
                        var client2 = bank2?.Clients.FirstOrDefault(cl => cl.Name == name && cl.Surname == surname);
                        if (bank2 != null && client2 != null)
                        {
                            var acc = service.CreateAccount(
                                client2, bank2.Id, account, money, DateTime.Today);
                            System.Console.WriteLine(acc.ToString());
                        }
                    }

                    break;

                case "4":
                    if (service.Banks == null) System.Console.WriteLine("There are no available banks and clients :(");
                    System.Console.WriteLine("What transaction do you want to do?");
                    System.Console.WriteLine("Replenish, Withdraw, Transfer");
                    var trans = System.Console.ReadLine();

                    System.Console.WriteLine("How much money?");
                    money = Convert.ToDecimal(System.Console.ReadLine());

                    System.Console.WriteLine("Please enter bank name:");
                    System.Console.WriteLine("Available banks:");
                    if (service.Banks != null)
                    {
                        foreach (var b in service.Banks)
                        {
                            System.Console.WriteLine($"{b.Name}");
                        }

                        bank = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter client name:");
                        name = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter client surname:");
                        surname = System.Console.ReadLine();

                        var bank1 = service.Banks.FirstOrDefault(b => b.Name == bank);
                        var client1 = bank1?.Clients.FirstOrDefault(cl => cl.Name == name && cl.Surname == surname);

                        System.Console.WriteLine("Please enter account type: ");
                        System.Console.WriteLine("Debit, Deposit, Credit");
                        account = System.Console.ReadLine();

                        var acc = bank1?.Accounts.FirstOrDefault(acc => acc.Owner == client1 && acc.AccountType == account);
                        System.Console.WriteLine("Making a transaction.....");

                        System.Console.WriteLine($"Money before: {acc?.Balance}");
                        switch (trans)
                        {
                            case "Replenish":
                                if (acc != null) bank1?.ReplenishMoney(acc.Id, money);
                                break;
                            case "Withdraw":
                                if (acc != null) bank1?.WithdrawMoney(acc.Id, money);
                                break;
                            case "Transfer":
                                System.Console.WriteLine("To what account do you want to transfer money?");
                                var to = System.Console.ReadLine();
                                System.Console.WriteLine("Enter client name:");
                                var name1 = System.Console.ReadLine();
                                System.Console.WriteLine("Enter client surname:");
                                var surname1 = System.Console.ReadLine();

                                System.Console.WriteLine("Please enter bank name:");
                                System.Console.WriteLine("Available banks:");
                                foreach (var b in service.Banks)
                                {
                                    System.Console.WriteLine($"{b.Name}");
                                }

                                bank = System.Console.ReadLine();
                                var bank2 = service.Banks.FirstOrDefault(b => b.Name == bank);
                                var accTo = bank2?.Accounts.FirstOrDefault(acc =>
                                    acc.Owner == client1 && acc.AccountType == account);
                                if (acc != null && accTo != null)
                                    bank1?.TransferMoney(acc.Id, accTo.Id, money);
                                break;
                        }

                        if (acc != null) System.Console.WriteLine($"Money now: {acc.Balance}");
                    }

                    break;

                case "5":
                    System.Console.WriteLine("Available banks:");
                    if (service.Banks != null)
                    {
                        foreach (var b in service.Banks)
                        {
                            System.Console.WriteLine($"{b.Name}");
                        }
                    }

                    break;

                case "6":
                    System.Console.WriteLine("Please enter bank name");
                    bank = System.Console.ReadLine();
                    if (service.Banks != null)
                        System.Console.WriteLine(service.Banks.FirstOrDefault(b => b.Name == bank)?.ToString());
                    break;

                case "7":
                    System.Console.WriteLine("Please enter bank name");
                    bank = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter client name:");
                    name = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter client surname:");
                    surname = System.Console.ReadLine();

                    System.Console.WriteLine("Please enter account type: ");
                    System.Console.WriteLine("Debit, Deposit, Credit");
                    account = System.Console.ReadLine();

                    if (service.Banks != null)
                    {
                        var bank1 = service.Banks.FirstOrDefault(b => b.Name == bank);
                        var client1 = bank1?.Clients.FirstOrDefault(cl => cl.Name == name && cl.Surname == surname);
                        System.Console.WriteLine(bank1?.Accounts.FirstOrDefault(acc => acc.Owner == client1 && acc.AccountType == account)?.ToString());
                    }

                    break;

                case "8":
                    break;

                default:
                    System.Console.WriteLine("Incorrect input, please try again");
                    break;
            }
        }
    }
}
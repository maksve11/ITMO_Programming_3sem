using Reports.BiLayer.Authentication_system;
using Reports.BiLayer.Message_processing_system;
using Reports.BiLayer.Reports_system;
using Reports.DALayer.Entities.Employee;
using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Models.Logger;

namespace Reports.PrLayer;

public class Program
{
    private static void Main()
    {
        string? line = string.Empty;
        string? name = string.Empty;
        string? surname = string.Empty;
        string? source = string.Empty;
        string? receiver = string.Empty;
        string? sender = string.Empty;
        EmployeeAccount account = null!;
        GuestAccount guest = null!;
        var messSysEE = new MessageManagerEE(new ConsoleLogger(true));
        var messSysEG = new MessageManagerEG(new ConsoleLogger(true));
        var messSysGE = new MessageManagerGE(new ConsoleLogger(true));
        var authSys = new AuthenticationManager(new ConsoleLogger(true));
        var repSys = new ReportService(new ConsoleLogger(true), authSys);
        while (line != "5")
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Welcome to Message Processing System!");
            System.Console.WriteLine("------------------------------------------------");
            System.Console.WriteLine("Please choose an action: ");
            System.Console.WriteLine("1 - create new Account(Guest or Employee)");
            System.Console.WriteLine("2 - log in to your account");
            System.Console.WriteLine("3 - send message");
            System.Console.WriteLine("4 - make Report");
            System.Console.WriteLine("5 - exit");
            System.Console.Write(">  ");
            line = System.Console.ReadLine();

            int tok1;
            switch (line)
            {
                case "1":
                    System.Console.WriteLine("Please choose type of Account: 1 - Guest, 2 - Employee");
                    string? typeAcc = System.Console.ReadLine();
                    if (typeAcc == "1")
                    {
                        System.Console.WriteLine("Write your name:");
                        name = System.Console.ReadLine();
                        System.Console.WriteLine("and Source(messenger, mail, mobile phone");
                        source = System.Console.ReadLine();
                        if (name != null && source != null)
                        {
                            guest = new GuestAccount(new MessengerSource(name, source));
                            System.Console.WriteLine("Your successfully create account!");
                            int token = authSys.Registration(guest);
                            System.Console.WriteLine("Please remember a token to log in to your account!");
                            System.Console.WriteLine(token);
                        }
                        else
                        {
                            System.Console.WriteLine("Encorrect values!");
                            break;
                        }
                    }
                    else if (typeAcc == "2")
                    {
                        System.Console.WriteLine("Write your name:");
                        name = System.Console.ReadLine();
                        System.Console.WriteLine("Write your surname:");
                        surname = System.Console.ReadLine();
                        System.Console.WriteLine("Set your status(Junior - 1, Middle - 2, TeamLead - 3:");
                        int status = Convert.ToInt16(System.Console.ReadLine());
                        source = System.Console.ReadLine();
                        if (name != null && source != null)
                        {
                            var employee = new Builder().SetNameAndSurname(name, surname).SetStatus(status).Build();
                            System.Console.WriteLine("Enter your login:");
                            string? login = System.Console.ReadLine();
                            System.Console.WriteLine("Enter your Password:");
                            string? password = System.Console.ReadLine();
                            if (login != null && password != null)
                            {
                                account = new EmployeeAccount(login, password, employee);
                                System.Console.WriteLine("Your successfully create account!");
                                int token = authSys.Registration(account);
                                System.Console.WriteLine("Please remember a token to log in to your account!");
                                System.Console.WriteLine(token);
                            }
                            else
                            {
                                System.Console.WriteLine("Encorrect values!");
                                break;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Encorrect values!");
                            break;
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Encorrect values!");
                        break;
                    }

                    break;
                case "2":
                    System.Console.WriteLine("Please enter your token:");
                    int tok = Convert.ToInt16(System.Console.ReadLine());
                    if (authSys.LogIn(tok))
                    {
                        System.Console.WriteLine("You successfully log in!");
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("Incorrect token, try again!");
                        break;
                    }

                case "3":
                    System.Console.WriteLine("Enter your token to log in");
                    tok1 = Convert.ToInt16(System.Console.ReadLine());
                    if (authSys.LogIn(tok1))
                    {
                        System.Console.WriteLine("You successfully log in!");
                        System.Console.WriteLine("Who do you want to send a message to?");
                        receiver = System.Console.ReadLine();
                        System.Console.WriteLine("Write your name like Author");
                        sender = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter who are you: Guest or Employee, just enter G or E:");
                        string? s = System.Console.ReadLine();
                        if (s == "G" && sender != null && receiver != null)
                        {
                            System.Console.WriteLine("Your receiver is Employee or other Guest? E/G");
                            string? g = System.Console.ReadLine();
                            if (g == "E")
                            {
                                System.Console.WriteLine("Please write your text:");
                                string? text = System.Console.ReadLine();
                                if (text != null)
                                {
                                    var message = messSysGE.SendMessage(text, guest, account);
                                    System.Console.WriteLine("Your message send successfully!");
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("You can't send message to other guest!");
                                break;
                            }
                        }
                        else if (s == "E" && sender != null && receiver != null)
                        {
                            System.Console.WriteLine("Your receiver is Employee or other Guest? E/G");
                            string? g = System.Console.ReadLine();
                            if (g == "E")
                            {
                                System.Console.WriteLine("Please write your text:");
                                string? text = System.Console.ReadLine();
                                if (text != null)
                                {
                                    var message = messSysEE.SendMessage(text, account, account);
                                    System.Console.WriteLine("Your message send successfully!");
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("Please write your text:");
                                string? text = System.Console.ReadLine();
                                if (text != null)
                                {
                                    var message = messSysEG.SendMessage(text, account, guest);
                                    System.Console.WriteLine("Your message send successfully!");
                                }
                            }
                        }

                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("Incorrect token, try again!");
                        break;
                    }

                case "4":
                    System.Console.WriteLine("Enter your token to log in");
                    tok1 = Convert.ToInt16(System.Console.ReadLine());
                    if (authSys.LogIn(tok1))
                    {
                        System.Console.WriteLine("You successfully log in!");
                        System.Console.WriteLine("Who do you want to send a message to?");
                        receiver = System.Console.ReadLine();
                        System.Console.WriteLine("Write your name like Author");
                        sender = System.Console.ReadLine();
                        System.Console.WriteLine("Please enter who are you: Guest or Employee, just enter G or E:");
                        string? s = System.Console.ReadLine();
                        if (s == "G" && sender != null && receiver != null)
                        {
                            System.Console.WriteLine("Your receiver is Employee or other Guest? E/G");
                            string? g = System.Console.ReadLine();
                            if (g == "E")
                            {
                                System.Console.WriteLine("Please write your text:");
                                string? text = System.Console.ReadLine();
                                if (text != null)
                                {
                                    var message = messSysGE.SendMessage(text, guest, account);
                                    System.Console.WriteLine("Your message send successfully!");
                                    messSysGE.SubmitForProcessing(message, account.Employee);
                                    var report = repSys.CreateReport(account, message);
                                    System.Console.WriteLine($"Report successfully created!: {report}");
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("You can't send message to other guest!");
                                break;
                            }
                        }
                        else if (s == "E" && sender != null && receiver != null)
                        {
                            System.Console.WriteLine("Your receiver is Employee or other Guest? E/G");
                            string? g = System.Console.ReadLine();
                            if (g == "E")
                            {
                                System.Console.WriteLine("Please write your text:");
                                string? text = System.Console.ReadLine();
                                if (text != null)
                                {
                                    var message = messSysEE.SendMessage(text, account, account);
                                    System.Console.WriteLine("Your message send successfully!");
                                    messSysEE.SubmitForProcessing(message, account.Employee);
                                    var report = repSys.CreateReport(account, message);
                                    System.Console.WriteLine($"Report successfully created!: {report}");
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("Please write your text:");
                                string? text = System.Console.ReadLine();
                                if (text != null)
                                {
                                    var message = messSysEG.SendMessage(text, account, guest);
                                    System.Console.WriteLine("Your message send successfully!");
                                    messSysEG.SubmitForProcessing(message, account.Employee);
                                    var report = repSys.CreateReport(account, message);
                                    System.Console.WriteLine($"Report successfully created!: {report}");
                                }
                            }
                        }

                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("Incorrect token, try again!");
                        break;
                    }

                case "5":
                    break;

                default:
                    System.Console.WriteLine("Incorrect input, please try again");
                    break;
            }
        }
    }
}
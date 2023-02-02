using Reports.BiLayer;
using Reports.BiLayer.Authentication_system;
using Reports.BiLayer.Message_processing_system;
using Reports.BiLayer.Reports_system;
using Reports.DALayer;
using Reports.DALayer.Entities.Employee;
using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Models.Logger;
using Xunit;

namespace Reports.Tests;

public class Reports_Tests
{
    [Fact]
    public void CreateEmployeeAccountAndSendMessageToOtherEmployee()
    {
        var logger = new ConsoleLogger(false);
        var messageManager = new MessageManagerEE(logger);
        var firstEmployee = new Builder().SetNameAndSurname("Oleg", "Tinkoff").SetStatus(2).Build();
        var account1 = new EmployeeAccount("oleshaTinok", "qwerty123", firstEmployee);
        firstEmployee.SetAccount(account1);
        var secondEmployee = new Builder().SetNameAndSurname("Doctor", "Strange").SetStatus(3).SetSubordinates(new List<Employee>() { firstEmployee }).Build();
        var account2 = new EmployeeAccount("drStrange", "sosibibu", secondEmployee);
        secondEmployee.SetAccount(account2);
        var authManager = new AuthenticationManager(logger);
        var token1 = authManager.Registration(account1);
        var token2 = authManager.Registration(account2);
        Assert.True(authManager.LogIn(token1));
        Assert.True(authManager.LogIn(token2));
        var message = messageManager.SendMessage("Ya tvoi rot naoborot", account1, account2);
        Assert.Contains(message, account2.Messages);
    }

    [Fact]
    public void CreateGuestAndSendMessageToEmployeeAndDoReport()
    {
        var logger = new ConsoleLogger(false);
        var messageManager = new MessageManagerGE(logger);
        var firstEmployee = new Builder().SetNameAndSurname("Oleg", "Tinkoff").SetStatus(2).Build();
        var account1 = new EmployeeAccount("oleshaTinok", "qwerty123", firstEmployee);
        firstEmployee.SetAccount(account1);
        var guest = new GuestAccount(new MessengerSource("Timur", "VK"));
        var authManager = new AuthenticationManager(logger);
        var token1 = authManager.Registration(account1);
        var token2 = authManager.Registration(guest);
        Assert.True(authManager.LogIn(token1));
        Assert.True(authManager.LogIn(token2));
        var message = messageManager.SendMessage("kak tvoi dela?", guest, account1);
        Assert.Contains(message, account1.Messages);
        var repSys = new ReportService(logger, authManager);
        var report = repSys.CreateReport(account1, message);
        Assert.NotNull(report);
    }
}
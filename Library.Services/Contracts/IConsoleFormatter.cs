using Library.Models.Models;
using System.Collections.Generic;

namespace Library.Services.Contracts
{
    public interface IConsoleFormatter
    {
        string Format(Book book);
        string Format(CheckoutBook book);
        string Format(ReservedBook book);
        string FormatList(List<CheckoutBook> books);
        string Format(User user);        
        string FormatListOfBooks(List<Book> books);
        string FormatCommandMessage(string message, string modelInfo);
        string FormatListOfUsers(List<User> users);
        string CenterStringWithSymbols(string text, char symbol);
    }
}

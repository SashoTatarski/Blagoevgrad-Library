using System;

namespace Library.Models.Utils
{
    public static class Constants
    {
        public const string DefaultRole = "user";

        // TODO: clean useless constants
        // Command names        
        public const string CheckOutBook = "Check Out Book";
        public const string AddBook = "Add Book";
        public const string LogIn = "Log In";
        public const string ReserveBook = "Reserve Book";
        public const string RegisterUser = "Register User";
        public const string RegisterLibrarian = "Register Librarian";
        public const string ReturnBook = "Return Book";
        public const string RemoveUser = "Remove User";
        public const string EditBook = "Edit Book";
        public const string RemoveBook = "Remove Book";
        public const string Search = "Search For Book";
        public const string Travel = "Travel In Time";
        public const string View = "View Accounts";

        // Static
        public static string NewLine = Environment.NewLine;

        // Commands     
        public const string NoSuchUserName = "No such username!";
        public const string InvalidPassword = "Wrong password!";
        public const string RemoveUserInvalidUserName = "Invalid username!";
        public const string ReserveBookAlreadyCheckedout = "Book is already checked out and reserved for another user afterwards!";
        public const string ReservedBookAlreadyReservedOther = "Book is already reserved by another user!";
        public const string ReservedBookAlreadyReserved = "You have already reserved this book!";
        public const string CheckoutBookAlreadyRes = "Book is already reserved!";
        public const string CheckoutBookAlreadyChecked = "Book is already checked out!";
        public const string ChooseBook = "Choose book by its ID";
        public const string ChooseUser = "Choose user by its username";
        public const string ChooseParameter = "Choose parameter";

        //Errors
        public const string UsernamePassIncorr = "Wrong username or password!";
        public const string UserIsBanned = "Your account has been banned!";
        public const string NoDefRoleDb = "No default role in DB";
        public const string NoSuchUser = "No such user!";
        public const string NoUsers = "There are no users!";
        public const string InvalidID = "Invalid ID!";
        public const string InvalidParameter = "Invalid parameter!";
        public const string CannotRemoveIssuedBook = "Cannot remove chechedout/reserved book!";
        public const string RemoveUserError = "Cannot remove user who has checkedout/reserved books!";
        public const string BookToUpdateNull = "Book to update is null";
        public const string UserNameTaken = "This username is already taken";
        public const string AuthorNameLimit = "The author name should be less than 40 symbols!";
        public const string PublisherNameLimit = "The publisher name should be less than 40 symbols!";
        public const string BookYearLimit = "The publication year should be between 1629 and 2019";
        public const string BookTitleLimit = "The title should be less than 40 symbols!";
        public const string BookRackLimit = "The rack number cannot be zero or negative!";
        public const string GenreNameLimit = "The genre name should be less than 40 symbols!";

        //Success
        public const string CheckoutBookSuccess = "Successfully checked of the book:";
        public const string EditBookSuccess = "Successfully edited the book:";
        public const string SuccessLogIn = "Successfully logged in as:";
        public const string SuccessLogOut = "Successfully logged out as:";
        public const string LibrarianRegisterSuccess = "Successfully created new librarian account:";
        public const string UserRegisterSuccess = "Successfully created new user account:";
        public const string RemoveBookSuccess = "Successfully removed the book:";
        public const string RemoveUserSuccess = "Successfully removed the user:";
        public const string ReturnBookSuccessMsg = "Successfully returned the book:";
        public const string ReservedBookSuccessMsg = "You have reserved the book:";
        public const string AddBookSuccess = "Successfully added the book:";
        public const string TravelSuccess = "Success! Today is";

        //Numbers
        public const decimal Fee = 0.5m;
        public const int MaxBookQuota = 5;
        public const int MaxCheckoutDays = 10;
        public const int MaxReserveDays = 5;
        public const int MaxFieldLength = 70;
        public const int ExtendPeriod = 5;
        public const int ExtendCost = 5;


        //Format
        public const char MiniDelimiterSymbol = '-';
        public const char DelimiterSymbol = '=';

        public static string Delimiter = new string(DelimiterSymbol, MaxFieldLength);
        public static string MiniDelimiter = new string(MiniDelimiterSymbol, MaxFieldLength);


        //Messages
        public const string SubscrOneMonth = "1 Month - $20";
        public const string SubscrOneYear = "1 Year - $200";
        public const string BookCreated = "{0} has been created!";
        public const string BookWrongIsbn = "{0} is associated with another book!";
        public const string BookCopiesAdded = "{0} copies have been added";
        public const string BookDeleted = "{0} has been deleted";
        public const string BookEdited = "{0} has been editted";
        public const string MaxQuotaReached = "You have reached the max book quota!";
        public const string Goodbye = "Goodbye!";
        public const string DaysToSkip = "how many days you want to skip";
        public const string Welcome = "Welcome to Blagoevgrad Library - Dimitar Talev -";
        public const string RetBookSucc = "Book Successfully Returned";
        public const string ResBookSucc = "Book Successfully Reserved";
        public const string ChBookSucc = "Book Successfully Checked Out!";
        public const string ChBookMax = "You Cannot Checkout More Than 5 Books";
        public const string AcctDeact = "Account has been deactived";
        public const string AcctBan = "Account has been banned";
        public const string UsernameValid = "Username should be at least 5 symbols";
        public const string PasswordValid = "Password should be at least 5 symbols";
        public const string AcctActivated = "{0} has been activated";
        public const string AcctDeactivated = "Your account has been deleted!";
        public const string AcctCancelErr = "You have to return all books before cancelling your account";
        public const string AcctCancel = "You have successfully cancelled your account";
        public const string AcctCancelRetBks = "User must return all of his books";
        public const string NoAvailableBooks = "There are no available book copies at the moment";
        public const string NotCheckedOutThisBook = "You are trying to return a book you have not checked out";
        public const string BookToBeDeleted = "This book will be removed soon";
        public const string NotEnoughMoney = "You do not have enough money in the wallet";
        public const string ExtendSuccess = "You have successfully extended the due date";
        public const string CancelReservationSuccess = "You have successfully cancel the reservation";
        public const string MembershipExpirationWarning = "Your membership is about to expire";
        public static string ExtendBookNotification = "{0} has extended the due date of {1} with id {2}";
        public static string CancelReservationNotification = "{0} has cancelled the reservation of {1} with id {2}";
        public static string CheckoutBookNotification = "{0} has checked out {1} with id {2}";
        public static string ReturnBookNotification = "{0} has returned {1} with id {2}";
        public static string CancelMembershipNotif = "{0} has cancelled his/her membership";
        public static string ReserveBookNotification = "{0} has reserved {1} with id {2}";
        public static string BookReview = "You have successfuuly reviewed your book";



        // Attribute messages
        public const string BookCopiesReqErr = "You must select number of copies";
        public const string BookCopiesReqRange = "You must select between 1 or 100 copies!";
        public const string AuthorsReqErr = "You must select an author";
        public const string PublishsReqErr = "You must select a publisher";
        public const string GenresReqErr = "You must select at least one genre";
        public const string TitleReqErr = "You must enter a title";
        public const string IsbnReqErr = "You must enter an ISBN";
        public const string YearReqErr = "You must select a valid year";



        public static string OverDueBookNotification = "You have an overdue book with title {0}. You are {1} day(s) overdue.";
        public static string OverDueMembershipNotification = "Your membership has expired on {0}";
        public const string OverDueMembershipAfterThreeDaysNotification = "Your membership will expire in 3 days";
        public const string OverDueMembershipAfterTwoDaysNotification = "Your membership will expire in 2 days";
        public const string OverDueMembershipAfterOneDayNotification = "Your membership will expire in 1 day";
        public const string OverdueReservation = "You have overdue reservation of book with title {0}. This book is no longer reserved by you.";



    }
}

namespace Library.Models.Enums
{
    public enum BookStatus
    {
        Available = 0,
        CheckedOut = 1,
        Reserved = 2,
        CheckedOutAndReserved = 3,
        ToBeDeleted = 4
    }
}

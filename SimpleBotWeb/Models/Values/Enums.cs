namespace SimpleBotWeb.Models.Values
{
    public enum UserRole
    {
        Administrator = 100,
        User = 0
    }

    public enum PasswordScore
    {
        Blank = 0,
        Bad1 = 1,
        Bad2 = 2,
        Weak1 = 3,
        Weak2 = 4,
        Medium1 = 5,
        Medium2 = 6,
        Strong1 = 7,
        Strong2 = 8,
        VeryStrong1 = 9,
        VeryStrong2 = 10
    }
}

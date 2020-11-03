using SimpleBotWeb.Models.Values;
using System;
using System.Text.RegularExpressions;

namespace SimpleBotWeb.Models.Helpers
{
    public class PasswordHelper
    {
        public const int MinimumLength = 6;
        public const int MediumLength = 10;
        public const int LongLength = 15;
        public const int VeryLongLength = 20;
        public const int SuperLongLength = 30;
        public const int IncredibleLength = 40;
        public const int HeroicLength = 50;

        public static PasswordScore CutoffStrength = PasswordScore.Weak1;

        public static PasswordScore GetPasswordStrength(string password)
        {
            int score = 0;

            if (string.IsNullOrWhiteSpace(password) || password.Length < MinimumLength)
                return 0;


            if (password.Length >= MediumLength)
                score++;

            if (password.Length >= LongLength)
                score++;

            if (password.Length >= VeryLongLength)
                score++;

            if (password.Length >= SuperLongLength)
                score++;

            if (password.Length >= IncredibleLength)
                score++;

            if (password.Length >= HeroicLength)
                score++;

            if (UpperCaseCount(password) >= 1)
                score++;

            if (LowerCaseCount(password) >= 1)
                score++;

            if (NumericCount(password) >= 1)
                score++;

            if (NonAlphaCount(password) >= 1)
                score += 2;

            // Maximum is 10
            if (score > 10)
                score = 10;
            return (PasswordScore)(score);
        }

        public static string TranslatePasswordScore(PasswordScore score)
        {
            switch (score)
            {
                case PasswordScore.Blank:
                    return "Passwords should be at least " + MinimumLength + " characters";
                case PasswordScore.Bad1:
                    return "Bad - Increase length, add numbers, upper and lowercase characters";
                case PasswordScore.Bad2:
                    return "Very weak";
                case PasswordScore.Weak1:
                    return "Weak";
                case PasswordScore.Weak2:
                    return "Weak";
                case PasswordScore.Medium1:
                    return "Medium";
                case PasswordScore.Medium2:
                    return "Medium";
                case PasswordScore.Strong1:
                    return "Strong";
                case PasswordScore.Strong2:
                    return "Strong";
                case PasswordScore.VeryStrong1:
                    return "Very strong";
                case PasswordScore.VeryStrong2:
                    return "Great!";
                default:
                    throw new ArgumentOutOfRangeException("score", score, null);
            }
        }

        public static bool IsValid(string password)
        {
            return GetPasswordStrength(password) >= CutoffStrength;
        }

        private static int UpperCaseCount(string password)
        {
            return Regex.Matches(password, "[A-Z]").Count;
        }

        private static int LowerCaseCount(string password)
        {
            return Regex.Matches(password, "[a-z]").Count;
        }

        private static int NumericCount(string password)
        {
            return Regex.Matches(password, "[0-9]").Count;
        }

        private static int NonAlphaCount(string password)
        {
            return Regex.Matches(password, @"[^0-9a-zA-Z\._]").Count;
        }
    }
}

using System;
using System.Text.RegularExpressions;

namespace CafeNoir.Domain.Models
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email nu poate fi gol");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
                throw new ArgumentException("Format email invalid");

            return new Email(email);
        }
    }
}
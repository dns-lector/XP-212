using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public record RomanNumber(int Value)
    {
        public static RomanNumber Parse(String input) => 
            RomanNumberParser.FromString(input);

        public override string? ToString()
        {
            Dictionary<int, String> ranges = new()
            {
                { 1,    "I"  },
                { 4,    "IV" },
                { 5,    "V"  },
                { 9,    "IX" },
                { 10,   "X"  },
                { 40,   "XL" },
                { 50,   "L"  },
                { 90,   "XC" },
                { 100,  "C"  },
                { 400,  "CD" },
                { 500,  "D"  },
                { 900,  "CM" },
                { 1000, "M"  },
            };

            if (Value == 0) { return "N"; }

            int number = Value;
            StringBuilder result = new();

            foreach (var range in ranges.Reverse())
            {
                while (number >= range.Key)
                {
                    result.Append(range.Value);
                    number -= range.Key;
                }
            }

            return result.ToString();
        }

        public double ToDouble() => (double) Value;
    }
}

/* Д.З. Створити методи класу RomanNumber, які перетворюють його
 * до різних числових типів: short, byte, long, int, float, double
 * Додати тестові методи, які перевіряють їх роботу.
 * 
 * 
 * 1234567
 * ""
 * "1" - ""
 * "12" - "1", ""
 * "123" - "12", "1", ""
 * "1234" - "123", "12", "1", ""
 * "12345" - "1234", "123", "12", "1", ""
 * "123456" - "12345", "1234", "123", "12", "1", "" (15)
 * "1234567" - "123456", "12345", "1234", "123", "12", "1", "" (21)
 * ~ n^2 / 2
 */

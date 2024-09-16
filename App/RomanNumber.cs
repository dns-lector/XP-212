using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumber(int Value)
    {
        private readonly int _value = Value;
        public int Value { get { return _value; } }

        public static RomanNumber Parse(String input) => 
            RomanNumberParser.FromString(input);
    }
}

/* Д.З. Створити тести для всіх методів класів 
 * RomanNumberValidator та RomanNumberParser
 * Розподілити тестові твердження (Assert) з загального тесту Parse
 * до тестів інших методів.
 */

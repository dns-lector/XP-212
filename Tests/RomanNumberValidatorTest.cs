using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class RomanNumberValidatorTest
    {
        [TestMethod]
        public void DigitValueTest()
        {
            Dictionary<char, int> testCases = new()
            {
                { 'N', 0    },
                { 'I', 1    },
                { 'V', 5    },
                { 'X', 10   },
                { 'L', 50   },
                { 'C', 100  },
                { 'D', 500  },
                { 'M', 1000 },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    RomanNumberValidator.DigitValue(testCase.Key),
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var testCase in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNumberValidator.DigitValue(testCase),
                    $"DigitValue('{testCase}') must throw ArgumentException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"'{testCase}'"),
                    "DigitValue ex.Message should contain a symbol which cause exception:" +
                    $" symbol: '{testCase}', ex.Message: '{ex.Message}'"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"{nameof(RomanNumber)}") &&
                    ex.Message.Contains($"DigitValue"),
                    "DigitValue ex.Message should contain name of class & name of method:" +
                    $" symbol: '{testCase}', ex.Message: '{ex.Message}'"
                );
            }
        }

        [TestMethod]
        public void CheckZeroDigitTest()
        {
            Object[][] exCases5 = [
                ["NN",   '0'],   // Цифра N не може бути у числі, тільки
                ["IN",   '1'],   // сама по собі
                ["NX",   '0'],
                ["NC",   '0'],
                ["XNC",  '1'],
                ["XVIN", '3'],
                ["XNNN", '1'],
            ];
            foreach (var exCase in exCases5)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumberValidator.CheckZeroDigit(exCase[0].ToString()!),
                    $"RomanNumberValidator.CheckZeroDigit(\"{exCase[0]}\") must throw FormatException"
                );
            }
        }
    }
}

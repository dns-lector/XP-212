using App;

namespace Tests
{
    [TestClass]
    public class RomanNumberTest 
    {
        private Dictionary<char, int> digits = new()
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

        [TestMethod]
        public void ToStringTest()
        {
            var testCases = new Dictionary<int, String>() {
                { 4, "IV" },
                { 6, "VI" },
                { 19, "XIX" },
                { 49, "XLIX" },
                { 95, "XCV" },
                { 444, "CDXLIV" },
                { 946, "CMXLVI" },
                { 3333, "MMMCCCXXXIII" },
            }
            .Concat(digits
                .Select(d => new KeyValuePair<int, string>(d.Value, d.Key.ToString()))
            );

            foreach (var testCase in testCases) 
            {
                RomanNumber rn = new(testCase.Key);
                var value = rn.ToString();
                Assert.IsNotNull(value);
                Assert.AreEqual(testCase.Value, value);
            }
        }

        [TestMethod]
        public void CrossTest_Parse_ToString()
        {
            // Наявність двох методів протилежної роботи дозволяє
            // використовувати крос-тести, які послідовно застосовують
            // два методи і одержують початковий результат
            // "XIX" -Parse-> 19 -ToString-> "XIX" v
            // "IIII" -Parse-> 4 -ToString-> "IV"  x
            // 4 -ToString-> "IV" -Parse-> 4   v
            for(int i = 0; i <= 1000; ++i)
            {
                int c = RomanNumberParser.FromString(
                        new RomanNumber(i).ToString()!
                    ).Value;

                Assert.AreEqual(
                    i,
                    c,
                    $"Cross test for {i}: {new RomanNumber(i)} -> {c}"
                );
            }
        }

        [TestMethod]
        public void ParseTest()
        {
            TestCase[] testCases = [
                new( "N",    0    ),
                new( "I",    1    ),
                new( "II",   2    ),
                new( "III",  3    ),
                new( "IIII", 4    ),  // цим кейсом ми дозволяємо неоптимальні форми
                new( "V",    5    ),
                new( "X",    10   ),
                new( "D",    500  ),
                new( "IV",   4    ),
                new( "VI",   6    ),
                new( "XI",   11   ),
                new( "IX",   9    ),
                new( "MM",   2000 ),
                new( "MCM",  1900 ),
            ];
            foreach (var testCase in testCases)
            {
                RomanNumber rn = RomanNumber.Parse(testCase.Source);
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Source}' is not null");
                Assert.AreEqual(
                    testCase.Value, 
                    rn.Value, 
                    $"Parse '{testCase.Source}' => {testCase.Value}"
                );
            }
            String tpl1 = "illegal symbol '%c'";
            String tpl2 = "in position %d";
            String tpl3 = "RomanNumber.Parse";
            testCases = [
                new( "W",   null, [ tpl1.Replace("%c", "W"), tpl2.Replace("%d", "0"), tpl3 ] ),
                new( "CS",  null, [ tpl1.Replace("%c", "S"), tpl2.Replace("%d", "1"), tpl3 ] ),
                new( "CX1", null, [ tpl1.Replace("%c", "1"), tpl2.Replace("%d", "2"), tpl3 ] ),
            ];
            foreach (var exCase in testCases)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase.Source),
                    $"RomanNumber.Parse(\"{exCase.Source}\") must throw FormatException"
                );
                foreach (String part in exCase.ExMessageParts!)
                {
                    Assert.IsTrue(
                        ex.Message.Contains(part),
                        $"ex.Message must contain '{part}'; ex.Message: {ex.Message}"
                    );
                }
            }

            Object[][] exCases2 = [
                [ "VX", 'V', 'X', 0 ],  // ---
                [ "LC", 'L', 'C', 0 ],  // "відстань" між цифрами при відніманні:
                [ "DM", 'D', 'M', 0 ],  // відніматись можуть I, X, C причому від
                [ "IC", 'I', 'C', 0 ],  // двох сусідніх цифр (I - від V та X, ...)
                [ "MIM", 'I', 'M', 1 ],  
                [ "MVM", 'V', 'M', 1 ],  
                [ "MXM", 'X', 'M', 1 ],  
                [ "CVC", 'V', 'C', 1 ],  
                [ "MCVC", 'V', 'C', 2 ],  
                [ "DCIC", 'I', 'C', 2 ],  
                [ "IM", 'I', 'M', 0 ],  // ---
            ];   
            foreach (var exCase in exCases2)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: '{exCase[1]}' before '{exCase[2]}'"),
                    $"ex.Message must contain symbols which cause error:'{exCase[1]}' and '{exCase[2]}'. " +
                    $"testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[3]}"),
                    $"ex.Message must contain error symbol position, testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber)) &&
                    ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                );
            }

            Object[][] exCases3 = [
                ["IIX", 'X', 2],   // Перед цифрою є декілька цифр, менших за неї
                ["VIX", 'X', 2],   // !! кожна пара цифр - правильна комбінація,
                ["XXC", 'C', 2],   //    проблема створюється щонайменше трьома цифрами
                ["IXC", 'C', 2],   // 
            ];
            foreach (var exCase in exCases3)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: more than one smaller digits before '{exCase[1]}'"),
                    $"ex.Message must contain symbol before error:'{exCase[1]}'. " +
                    $"testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[2]}"),
                    $"ex.Message must contain error symbol position, testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber)) &&
                    ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, testCase: '{exCase[0]}', ex.Message: {ex.Message}"
                );
            }

            Object[][] exCases4 = [
                ["IXX",  'I'],   // Менша цифра після двох однакових
                ["IXXX", 'I'],   // 
                ["IXIX", 'I'],   // 
                ["XCC",  'X'],   // 
                ["XCXC", 'X'],  
                ["IVIV", 'I'],  
                ["XCCC", 'X'],  
                ["CXCC", 'X'],  
                ["CMM",  'C'],   
                ["CMMM", 'C'],  
                ["MCMM", 'C'],  
            ];
            foreach (var exCase in exCases4)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
            }

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
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
            }
        }

        [TestMethod]
        public void DigitValueTest()
        {            
            foreach (var testCase in digits)
            {
                Assert.AreEqual(
                    testCase.Value, 
                    RomanNumberParser.DigitValue(testCase.Key), 
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var testCase in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNumberParser.DigitValue(testCase),
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
    }

    
    record TestCase(String Source, int? Value, IEnumerable<String>? ExMessageParts = null )
    {
       
    }

}

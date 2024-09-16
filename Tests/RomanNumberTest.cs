using App;

namespace Tests
{
    [TestClass]
    public class RomanNumberTest  
    {
        [TestMethod]
        public void ParseTest()
        {
            Dictionary<String, int> testCases = new()
            {
                { "N",    0    },
                { "I",    1    },
                { "II",   2    },
                { "III",  3    },
                { "IIII", 4    },  // ��� ������ �� ���������� ����������� �����
                { "V",    5    },
                { "X",    10   },
                { "D",    500  },
                { "IV",   4    },
                { "VI",   6    },
                { "XI",   11   },
                { "IX",   9    },
                { "MM",   2000 },
                { "MCM",  1900 },
            };
            foreach (var testCase in testCases)
            {
                RomanNumber rn = RomanNumber.Parse(testCase.Key);
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Key}' is not null");
                Assert.AreEqual(
                    testCase.Value, 
                    rn.Value, 
                    $"Parse '{testCase.Key}' => {testCase.Value}"
                );
            }

            /* ������� ������� - ���� ������� ������� ������ �������
             * ��� ���� ���������� ������� (������� � �����)
             */
            Object[][] exCases = [
                [ "W",   'W', 0 ],
                [ "CS",  'S', 1 ],
                [ "CX1", '1', 2 ],
            ];
            foreach (var exCase in exCases)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(exCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{exCase[0]}\") must throw FormatException"
                );
                // ��������� ������ �� �����������
                // - �� ������ ��� ������, �� ���������� �� �������
                // - �� ������ ������� ������� � �����
                // - �� ������ ����� ������ �� �����
                Assert.IsTrue(
                    ex.Message.Contains($"illegal symbol '{exCase[1]}'"),
                    $"ex.Message must contain symbol which cause error:" +
                    $" '{exCase[1]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {exCase[2]}"),
                    $"ex.Message must contain error symbol position, ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber)) &&
                    ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, ex.Message: {ex.Message}"
                );
            }

            /* ����� �� ����������� ���������� ����� (�� ����� ��������,
             * ��� ����������� �� �����������)
             * VV, VX, IIX, ...
             */
            Object[][] exCases2 = [
                [ "VX", 'V', 'X', 0 ],  // ---
                [ "LC", 'L', 'C', 0 ],  // "�������" �� ������� ��� �������:
                [ "DM", 'D', 'M', 0 ],  // ��������� ������ I, X, C ������� ��
                [ "IC", 'I', 'C', 0 ],  // ���� ������ ���� (I - �� V �� X, ...)
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
                ["IIX", 'X', 2],   // ����� ������ � ������� ����, ������ �� ��
                ["VIX", 'X', 2],   // !! ����� ���� ���� - ��������� ���������,
                ["XXC", 'C', 2],   //    �������� ����������� ���������� ������ �������
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
            /* �.�. 
             * - ��������� ������ ����� ��� ��� ����� �����, ��������
             *     ����� � ������ ����������� �� ��� ����� (������(�), ������� ����)
             * - ���������� ����������� ����� �� ���� ����������� �����������
             *     ������� � ������� ������� ������� (exCases3)
             */

            Object[][] exCases4 = [
                ["IXX",  'I'],   // ����� ����� ���� ���� ���������
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
                ["NN",   '0'],   // ����� N �� ���� ���� � ����, �����
                ["IN",   '1'],   // ���� �� ���
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
                    RomanNumber.DigitValue(testCase.Key), 
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var testCase in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNumber.DigitValue(testCase),
                    $"DigitValue('{testCase}') must throw ArgumentException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"'{testCase}'"),
                    "DigitValue ex.Message should contain a symbol which cause exception:" +
                    $" symbol: '{testCase}', ex.Message: '{ex.Message}'"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"{nameof(RomanNumber)}") &&
                    ex.Message.Contains($"{nameof(RomanNumber.DigitValue)}"),
                    "DigitValue ex.Message should contain name of class & name of method:" +
                    $" symbol: '{testCase}', ex.Message: '{ex.Message}'"
                );
            }
        }
    }
}
/* �.�. ������ ������ ����� �� ����� � ������������ ������.
 * ����������� �������� ������� � ������� ������������� �������
 * �� ������� "SDR". ��������, ��� ���������� ������� ������� � 
 * �������, �� ��������� �������� ('S' pos 0)
 * ** ��������, ��� ����������� ��� ������� ������ �Ѳ ���������� 
 *    ����� �� �� �������.
 */

/* �������� ����� �� ���������� �������� �������� �����:
 * - ���� ����� ���������� ������ ��������� ������
 * - ���� ����� ��������� �� � ������, � ������� Test
 * - ������ ����� ����� ���������� ������ ������������ ����� 
 *     � ����� � ������� Test
 *     
 * ������ ����� ��������� ������� (Assert)
 * ���� ��������� ��������� ���� �� ���� ������� ������
 * ���������� - ���� ���� � ���� ���� ������
 * 
 */

using RPN;
using System;

namespace RPNTester {
    public class Program {
        private static readonly RPNStack rpn = new();
        private static readonly int cw1 = rpn.ColumnWidth;
        private static readonly int cw2 = cw1 - 1;
        private static readonly int maxStack = 4;
        private static int sectionIndex = 0;

        public static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ClearScreen(false);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            //Console.BackgroundColor = ConsoleColor.Green;
            //Console.ForegroundColor = ConsoleColor.Black;

            //var r = rpn.Push(rpn.InfixToRPN("X+(COS((X)))"));
            //var r = rpn.Push(rpn.InfixToRPN("( 1 + (27) ) * ( 3 / 4 ) ^ ( 5 + 6 )"));
            //var r = rpn.Push("a b + c d + / x y -");
            //var r = rpn.Push("1 a 1 + b 1 + / /");
            //var r = rpn.Push("a b + 'y+3' ^ x /");
            //var r = rpn.Push("'1/5^3' DUP EVAL");
            //Debugger.Break();

            while(true) {
                RefreshScreen(maxStack);

                bool stringMode = false;
                string newEntry = "";
                while(true) {
                    ConsoleKeyInfo k = Console.ReadKey(true);
                    ConsoleKey key = k.Key;

                    if(key == ConsoleKey.Oem7) { // '
                        stringMode = !stringMode;
                    }

                    if(key == ConsoleKey.Tab) {
                        sectionIndex++;
                        break;
                    }

                    if(key == ConsoleKey.Enter) {
                        if(newEntry == "") {
                            rpn.Push("DUP");
                        } else {
                            rpn.Push(newEntry);
                        }
                        break;
                    }

                    if(key == ConsoleKey.Backspace) {
                        if(newEntry == "") {
                            rpn.Push("DROP");
                            break;
                        } else {
                            newEntry = newEntry[0..^1];
                            if(newEntry == "") break; // I personally don't like this behavior,
                                                      // but that's how the HP48 behaves.
                        }
                    }

                    if(key == ConsoleKey.Escape) {
                        ClearScreen(true);
                        return;
                    }

                    char kc = k.KeyChar;
                    string kcs = kc.ToString();
                    if(!stringMode && rpn.IsOpCode(kcs)) {
                        if(newEntry != "") rpn.Push(newEntry);
                        rpn.Push(kcs);
                        break;
                    }

                    if(char.IsLetterOrDigit(kc) ||
                       char.IsSymbol(kc) ||
                       char.IsPunctuation(kc) ||
                       char.IsWhiteSpace(kc)) {
                        newEntry += kcs;
                    }

                    rpn.ResetErrorState();
                    RefreshScreen(maxStack - 1);

                    string edl = newEntry.Length < cw1 ? newEntry : "…" + newEntry[(newEntry.Length - cw2 + 1)..];
                    Console.WriteLine($"{edl}◄{"".PadRight(cw2 - edl.Length)}");
                }
            }
        }

        private static void ClearScreen(bool cursorVisible) {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.CursorVisible = cursorVisible;
        }

        private static void RefreshScreen(int stackSize) {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(rpn.ErrorFunction.PadRight(cw1));
            Console.WriteLine(rpn.ErrorMessage.PadRight(cw1));
            rpn.PrintStack(stackSize);
            int cpy = Console.CursorTop;

            while(stackSize++ <= maxStack) {
                Console.WriteLine();
            }
            rpn.PrintVariables();

            Console.WriteLine("\n");
            sectionIndex = rpn.PrintOpCodes(sectionIndex);

            Console.SetCursorPosition(0, cpy);
        }
    }
}
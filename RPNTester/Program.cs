using RPN;
using System;

namespace RPNTester {
    public class Program {
        private static readonly RPNStack rpn = new();
        private static readonly int cw1 = rpn.ColumnWidth;
        private static readonly int cw2 = cw1 - 1;

        public static void Main(string[] args) {
            ClearScreen(false);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            int maxStack = 4;

            //var r = rpn.Push(rpn.InfixToRPN("X+(COS((X)))"));
            //var r = rpn.Push(rpn.InfixToRPN("( 1 + (27) ) * ( 3 / 4 ) ^ ( 5 + 6 )"));
            //Debugger.Break();
            //var r = rpn.Push("'1/5^3'");

            while(true) {
                RefreshScreen(maxStack);

                bool stringMode = false;
                string newEntry = "";
                while(true) {
                    ConsoleKeyInfo k = Console.ReadKey(true);
                    ConsoleKey key = k.Key;

                    if(key == ConsoleKey.E) { // E = Eval
                        foreach(string token in rpn.InfixToRPN(rpn.Pop()).Split(' '))
                            rpn.Push(token);
                        break;
                    }

                    if(key == ConsoleKey.Oem7) { // '
                        stringMode = !stringMode;
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

        private static void RefreshScreen(int maxStack) {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(rpn.ErrorFunction.PadRight(cw1));
            Console.WriteLine(rpn.ErrorMessage.PadRight(cw1));
            rpn.PrintStack(maxStack);
        }
    }
}
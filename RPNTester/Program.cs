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

            while(true) {
                RefreshScreen(maxStack);

                string newEntry = "";
                while(true) {
                    ConsoleKeyInfo k = Console.ReadKey(true);
                    if(k.Key == ConsoleKey.Enter) {
                        rpn.Push(newEntry);
                        break;
                    }
                    if((k.Key == ConsoleKey.Delete) && (newEntry == "")) {
                        rpn.Push("DROP");
                        break;
                    }
                    if((k.Key == ConsoleKey.Backspace)) {
                        newEntry = newEntry[0..^1];
                    }
                    if(k.Key == ConsoleKey.Escape) {
                        ClearScreen(true);
                        return;
                    }
                    if(rpn.IsFunction(k.KeyChar.ToString())) {
                        rpn.Push(newEntry);
                        rpn.Push(k.KeyChar.ToString());
                        break;
                    }

                    if(char.IsLetterOrDigit(k.KeyChar) || char.IsSymbol(k.KeyChar) || char.IsPunctuation(k.KeyChar)) {
                        newEntry += k.KeyChar;
                    }

                    rpn.ResetErrorState();
                    RefreshScreen(maxStack - 1);

                    string tmp = newEntry.Length < cw1 ? newEntry : "…" + newEntry[(newEntry.Length - cw2 + 1)..];
                    Console.WriteLine($"{tmp}◄{"".PadRight(cw2 - tmp.Length)}");
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
using RPN;
using System;

namespace RPNTester {
    public class Program {
        private static readonly RPNStack rpn = new();
        private static int cw1 = rpn.ColumnWidth;
        private static int cw2 = cw1 - 1;

        public static void Main(string[] args) {
            Console.CursorVisible = false;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            int maxStack = 4;

            while(true) {
                Refresh(maxStack);

                string newEntry = "";
                while(true) {
                    ConsoleKeyInfo k = Console.ReadKey(true);
                    if(k.Key == ConsoleKey.Enter) {
                        rpn.Push(newEntry);
                        break;
                    }
                    if(k.Key == ConsoleKey.Escape) return;

                    if(rpn.IsFunction(k.KeyChar.ToString())) {
                        rpn.Push(newEntry);
                        rpn.Push(k.KeyChar.ToString());
                        break;
                    }

                    newEntry += k.KeyChar;

                    rpn.ResetErrorState();
                    Refresh(maxStack - 1);

                    string tmp = newEntry.Length < cw1 ? newEntry : "…" + newEntry.Substring(newEntry.Length - cw2 + 1);
                    Console.WriteLine($"{tmp}◄{"".PadRight(cw2 - tmp.Length)}");
                }
            }

            //PushAndPrint("5");
            //PushAndPrint("3");
            //PushAndPrint("2");
            //PushAndPrint("-");
            //PushAndPrint("-");
            //PushAndPrint("6");
            //PushAndPrint("+");

            //PushAndPrint("3");
            //PushAndPrint("2");
            //PushAndPrint("SWAP");
            //PushAndPrint("/");
            //PushAndPrint("*");

            //PushAndPrint("SQRT");
            //PushAndPrint("√");

            Console.CursorVisible = true;
        }

        private static void Refresh(int maxStack) {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(rpn.ErrorFunction.PadRight(cw1));
            Console.WriteLine(rpn.ErrorMessage.PadRight(cw1));
            rpn.PrintStack(maxStack);
        }

        public static void PushAndPrint(string value) {
            Console.SetCursorPosition(0, 0);
            rpn.Push(value);
            rpn.PrintStack();
            Console.WriteLine($"\nInput: {value,-20}");
            Console.ReadKey(true);
        }
    }
}
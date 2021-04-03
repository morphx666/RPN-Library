using RPN;
using System;

namespace RPNTester {
    public class Program {
        private static readonly RPNStack rpn = new();

        public static void Main(string[] args) {
            Console.CursorVisible = false;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;

            int cw1 = rpn.ColumnWidth;
            int cw2 = cw1 - 1;
            int maxStack = 8;

            while(true) {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"{"".PadRight(cw1)}"); // Error Function (not implemented)
                Console.WriteLine($"{"".PadRight(cw1)}"); // Error Description (not implemented)
                rpn.PrintStack(maxStack);

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

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(""); // Error Function
                    Console.WriteLine(""); // Error Description
                    rpn.PrintStack(maxStack - 1);

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

        public static void PushAndPrint(string value) {
            Console.SetCursorPosition(0, 0);
            rpn.Push(value);
            rpn.PrintStack();
            Console.WriteLine($"\nInput: {value,-20}");
            Console.ReadKey(true);
        }
    }
}
using RPN;
using System;

namespace RPNTester {
    public class Program {
        private static readonly RPNStack rpn = new();
        private static readonly int cw1 = rpn.ColumnWidth;
        private static readonly int cw2 = cw1 - 1;
        private static readonly int maxStack = 4;
        private static int sectionIndex = 0;
        private static int opCodeIndex = 0;
        private static string selectedOpCode = "";

        public static void Main(string[] args) {
            Console.Title = "RPN Library Tester";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ClearScreen(false);
            ResetColors();

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

                    if(key == ConsoleKey.RightArrow) {
                        if(newEntry == "") rpn.Push("SWAP");
                        break;
                    }

                    if(key == ConsoleKey.PageUp) {
                        opCodeIndex = 0;
                        sectionIndex--;
                        break;
                    }

                    if(key == ConsoleKey.PageDown) {
                        opCodeIndex = 0;
                        sectionIndex++;
                        break;
                    }

                    if(key == ConsoleKey.Tab) {
                        opCodeIndex += (k.Modifiers == ConsoleModifiers.Shift) ? -1 : 1;
                        break;
                    }

                    if(key == ConsoleKey.Enter) {
                        if(k.Modifiers == ConsoleModifiers.Control) {
                            if(newEntry != "") rpn.Push(newEntry);
                            rpn.Push(selectedOpCode);
                        } else {
                            if(newEntry == "") {
                                rpn.Push("DUP");
                            } else {
                                rpn.Push(newEntry);
                            }
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
            (sectionIndex, opCodeIndex, selectedOpCode) = rpn.PrintOpCodes(sectionIndex, opCodeIndex);

            PrintHelp();

            Console.SetCursorPosition(0, cpy);
        }

        private static void PrintHelp() {
            string pushInfo = $"Ctrl+Enter:    Push Selected OpCode [{selectedOpCode}]";
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorTop -= 2;
            Console.Write("PageUp/PageDn: Select Section\n");
            Console.Write("Tab:           Select OpCode\n");
            Console.Write($"{pushInfo,-60}");
            ResetColors();
        }

        private static void ResetColors() {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            //Console.BackgroundColor = ConsoleColor.Green;
            //Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
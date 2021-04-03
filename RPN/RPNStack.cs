using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN {
    public class RPNStack {
        public int ColumnWidth { get; init; }

        public string ErrorFunction { get; internal set; } = "";
        public string ErrorMessage { get; internal set; } = "";

        private readonly Stack<string> stack;
        private readonly List<Function> functions;

        public RPNStack(int columnWidth = 22) {
            stack = new();
            functions = Function.GetAvailableFunctions();
            this.ColumnWidth = columnWidth;
        }

        public void PrintStack(int max = 4) {
            string[] stk = new string[max];
            string tmp;
            string idx;
            Array.Copy(stack.ToArray(), stk, stack.Count);
            for(int i = max - 1; i >= 0; i--) {
                idx = (i + 1).ToString();
                tmp = stk[i] ?? "";
                tmp = tmp.Length <= ColumnWidth ?
                        tmp :
                        "…" + stk[i].Substring(stk[i].Length - ColumnWidth + idx.Length + 2);
                Console.Write($"{tmp.PadLeft(ColumnWidth)}");
                Console.CursorLeft = 0;
                Console.WriteLine(idx + ":");
            }
        }

        public bool IsFunction(string obj) {
            bool isFunction = false;
            functions.ForEach(f => {
                if(f.Symbols.Contains(obj)) {
                    isFunction = true;
                    return;
                }
            });

            return isFunction;
        }

        public void ResetErrorState() {
            ErrorFunction = "";
            ErrorMessage = "";
        }

        public bool Push(string obj) {
            bool isFunction = false;

            ResetErrorState();

            if(obj != "") {
                stack.Push(obj);

                functions.ForEach(f => {
                    if(f.Symbols.Contains(obj)) {
                        stack.Pop();
                        if(!f.Execute(stack)) {
                            ErrorFunction = f.ErrorFunction;
                            ErrorMessage = f.ErrorMessage;
                            return;
                        }
                        isFunction = true;
                        return;
                    }
                });
            }

            return isFunction;

            // TODO: Validate 'value' is a valid digit / constant (not implemented) / variable (not implemented)
        }
    }
}

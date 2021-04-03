using RPN.Functions;
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
            Array.Copy(stack.ToArray(), 0, stk, 0, Math.Min(max, stack.Count));
            for(int i = max - 1; i >= 0; i--) {
                idx = (i + 1).ToString();
                tmp = stk[i] ?? "";
                tmp = tmp.Length <= ColumnWidth ?
                        tmp :
                        "…" + stk[i][(stk[i].Length - ColumnWidth + idx.Length + 2)..];
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
            bool hasErrors = false;

            ResetErrorState();

            if(obj != "") {
                functions.ForEach(f => {
                    if(f.Symbols.Contains(obj)) {
                        if(!f.Execute(stack)) {
                            // FIXME: This error handling sucks. There must be a better way.
                            ErrorFunction = f.ErrorFunction;
                            ErrorMessage = f.ErrorMessage;
                            hasErrors = true;
                            return;
                        }
                        isFunction = true;
                        return;
                    }
                });

                if(!isFunction && !hasErrors) {
                    obj = obj.Replace("'", "");
                    try {
                        double tmp = double.Parse(obj);
                        stack.Push(obj);
                    } catch { // FIXME: If it fails then it must be a string.
                              // Of course, this will require a more robust parsing algorithm if we want
                              // to support more object types, besides numbers and strings.
                        stack.Push($"'{obj}'");
                    }
                }
            }

            return isFunction;

            // TODO: Validate 'value' is a valid digit / constant (not implemented) / variable (not implemented)
        }
    }
}

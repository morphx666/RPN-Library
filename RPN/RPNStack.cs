using RPN.OpCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using static RPN.OpCodes.OpCode;

namespace RPN {
    public class RPNStack {
        public int ColumnWidth { get; init; }

        public string ErrorFunction { get; internal set; } = "";
        public string ErrorMessage { get; internal set; } = "";

        private readonly Stack<string> stack;
        private readonly List<OpCode> functions;

        public RPNStack(int columnWidth = 22) {
            stack = new();
            functions = OpCode.GetAvailableFunctions();
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

            ResetErrorState();

            foreach(string token in obj.Split(' ')) {
                isFunction |= ParseToken(token);
            }
            return isFunction;
        }

        private bool ParseToken(string token) {
            bool isFunction = false;
            bool hasErrors = false;

            if(token != "") {
                functions.ForEach(f => {
                    if(f.Symbols.Contains(token)) {
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
                    Types dataType = RPN.OpCodes.OpCode.InferType(token);
                    if((dataType & Types.String) == Types.String) {
                        stack.Push($"\"{token.Replace("\"", "")}\"");
                    } else if(((dataType & Types.String) == Types.String) ||
                       ((dataType & Types.Formula) == Types.Formula)) {
                        stack.Push($"'{token.Replace("'", "")}'");
                    } else {
                        try {
                            double tmp = double.Parse(token);
                            stack.Push(token);
                        } catch { // FIXME: If it fails then it must be a string.
                                  // Of course, this will require a more robust parsing algorithm if we want
                                  // to support more object types, besides numbers and strings.
                            stack.Push($"'{token}'");
                        }
                    }
                }
            }

            return isFunction;

            // TODO: Validate 'value' is a valid digit / constant (not implemented) / variable (not implemented)
        }
    }
}
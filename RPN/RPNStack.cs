﻿using RPN.OpCodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static RPN.OpCodes.OpCode;

namespace RPN {
    public class RPNStack {
        public record StackItem {
            public string Token { get; init; }
            public Types Type { get; init; }
            public bool HasDelimeters { get; init; }

            public string AsString() {
                return Type switch {
                    Types.Infix => $"'{Token}'",
                    Types.String => $"\"{Token}\"",
                    _ => Token,
                };
            }

            public StackItem(string token, Types type) {
                if(token == null) {
                    Token = "";
                } else {
                    switch(type) {
                        case Types.Infix:
                            Token = token.Replace("'", "");
                            break;
                        case Types.String:
                            Token = token.Replace("\"", "");
                            break;
                        default:
                            Token = token;
                            break;
                    }
                }
                Type = type;
                HasDelimeters = type == Types.Infix || type == Types.String;
            }
        }

        public int ColumnWidth { get; init; }

        public string ErrorFunction { get; internal set; } = "";
        public string ErrorMessage { get; internal set; } = "";

        private readonly Stack<StackItem> stack;
        private readonly List<OpCode> opCodes;
        private readonly bool SimplifyTokens;

        public RPNStack(int columnWidth = 22, bool simplifyTokens = true) {
            stack = new();
            ColumnWidth = columnWidth;
            opCodes = GetAvailableOpCodes();
            SimplifyTokens = simplifyTokens;
        }

        public void PrintStack(int max = 4, bool showTypes = false) {
            StackItem[] stk = new StackItem[max];
            Array.Copy(stack.ToArray(), 0, stk, 0, Math.Min(max, stack.Count));

            string tmp;
            string idx;
            int cw;
            for(int i = max - 1; i >= 0; i--) {
                idx = (i + 1).ToString();

                if(stk[i] == null) {
                    cw = ColumnWidth;
                    tmp = "";
                } else {
                    cw = ColumnWidth - (stk[i].HasDelimeters ? 2 : 0);
                    tmp = stk[i].AsString();
                }

                tmp = tmp.Length <= cw ?
                        tmp :
                        "…" + tmp[(tmp.Length - cw + idx.Length + 2)..];
                Console.Write($"{tmp.PadLeft(ColumnWidth)}");
                if(showTypes) {
                    if(tmp != "") tmp = stk[i].Type.ToString();
                    Console.Write($"|{tmp,10}");
                }
                Console.CursorLeft = 0;
                Console.WriteLine(idx + ":");
            }
        }

        public bool IsOpCode(string token) {
            bool isFunction = false;
            foreach(OpCode oc in opCodes) {
                if(oc.Symbols.Contains(token)) {
                    isFunction = true;
                    break;
                }
            }

            return isFunction;
        }

        public OpCode GetOpCode(string token) {
            foreach(OpCode oc in opCodes) {
                if(oc.Symbols.Contains(token)) {
                    return oc;
                }
            }

            return null;
        }

        private bool ContainsOpCodes(List<string> tokens) {
            foreach(string token in tokens) {
                if(IsOpCode(token)) return true;
            }
            return false;
        }

        public void ResetErrorState() {
            ErrorFunction = "";
            ErrorMessage = "";
        }

        public int Count {
            get => stack.Count;
        }

        public StackItem[] ToArray() {
            return stack.ToArray();
        }

        public void Clear() {
            stack.Clear();
        }

        public StackItem Pop() {
            return stack.Pop();
        }

        public StackItem Peek() {
            return stack.Peek();
        }

        public void CopyTo(StackItem[] destination) {
            stack.CopyTo(destination, 0);
        }

        public bool Push(StackItem item) {
            return Push(item.Token, item.Type);
        }

        public bool Push(string tokens, Types dataType = Types.Any) {
            ResetErrorState();
            if(tokens == "") return false;

            bool isFunction = false;
            foreach(string subToken in tokens.Split(' ')) {
                isFunction |= ParseToken(subToken, dataType);
            }
            return isFunction;
        }

        private bool ParseToken(string token, Types dataType = Types.Any) {
            bool isOpCode = false;
            bool hasErrors = false;

            if(dataType == Types.Number) Debugger.Break();

            if(token != "") {
                foreach(OpCode oc in opCodes) {
                    if(oc.Symbols.Contains(token)) {
                        if(!oc.Execute(this)) {
                            // FIXME: This error handling sucks. There must be a better way.
                            ErrorFunction = oc.ErrorFunction;
                            ErrorMessage = oc.ErrorMessage;
                            hasErrors = true;
                            break;
                        }
                        isOpCode = true;
                        break;
                    }
                }

                if(!isOpCode && !hasErrors) {
                    if(dataType == Types.Any) dataType = InferType(token);
                    if(dataType == Types.Infix) {
                        try {
                            stack.Push(new StackItem(Simplify(token), Types.Infix));
                        } catch {
                            ErrorFunction = "";
                            ErrorMessage = "Invalid Syntax";
                        }
                    } else {
                        //stack.Push(new StackItem(token.Replace("'", ""), dataType));
                        stack.Push(new StackItem(token, dataType));
                    }
                }
            }

            return isOpCode;

            // TODO: Validate 'value' is a valid digit / constant (not implemented) / variable (not implemented)
        }

        private string Simplify(string token) {
            if(!SimplifyTokens) return token;

            string infix = InfixToRPN(token);
            if(infix == token) {
                return $"{token}";
            } else {
                return $"{RPNToInfix(infix)}";
            }
        }

        // https://codetocreate.wordpress.com/converting-an-expression-to-rpn/
        public string InfixToRPN(string exp) { // Shunting-yard algorithm
            List<string> tokens = Tokenize(exp);

            List<string> output = new();
            Stack<string> stack = new();

            // For all the input tokens [S1] read the next token [S2]
            foreach(string token in tokens) {
                OpCode opCode = GetOpCode(token);
                if(opCode != null) {
                    // Token is an operator [S3]
                    while((stack.Count != 0) && (GetOpCode(stack.Peek()) != null)) {
                        // While there is an operator (y) at the top of the operators stack and 
                        // either (x) is left-associative and its precedence is less or equal to 
                        // that of (y), or (x) is right-associative and its precedence 
                        // is less than (y)
                        // 
                        // [S4]:
                        OpCode cOp = GetOpCode(token); // Current operator
                        OpCode lOp = GetOpCode(stack.Peek()); // Top operator from the stack
                        if((cOp.Associativity == Associativities.Left && cOp.ComparePrecedence(lOp) <= 0) ||
                           (cOp.Associativity == Associativities.Right && cOp.ComparePrecedence(lOp) < 0)) {
                            // Pop (y) from the stack S[5]
                            // Add (y) output buffer S[6]
                            output.Add(stack.Pop());
                            continue;
                        }
                        break;
                    }
                    // Push the new operator on the stack S[7]
                    stack.Push(token);
                } else if(token == "(") {
                    // Else If token is left parenthesis, then push it on the stack S[8]
                    stack.Push(token);
                } else if(token == ")") {
                    // Else If the token is right parenthesis S[9]
                    while((stack.Count != 0) && (stack.Peek() != "(")) {
                        // Until the top token (from the stack) is left parenthesis, pop from 
                        // the stack to the output buffer
                        // S[10]
                        output.Add(stack.Pop());
                    }
                    // Also pop the left parenthesis but don't include it in the output 
                    // buffer S[11]
                    stack.Pop();
                } else {
                    // Else add token to output buffer S[12]
                    output.Add(token);
                }
            }

            while(stack.Count != 0) {
                // While there are still operator tokens in the stack, pop them to output S[13]
                output.Add(stack.Pop());
            }

            return string.Join(' ', output.ToArray());
        }

        private List<string> Tokenize(string exp) {
            List<string> tokens = new();
            string v = "";
            char c;
            int mode = 0; // 0 = Number
                          // 1 = String

            bool isNumber(int i) => char.IsDigit(exp[i]) || exp[i] == '.' || (i == 0 && (exp[i] == '+' || exp[i] == '-'));

            mode = isNumber(0) ? 0 : 1;

            for(int i = 0; i < exp.Length; i++) {
                c = exp[i];
                if(char.IsWhiteSpace(c)) continue;

                if(c == '(') {
                    if(!string.IsNullOrEmpty(v)) tokens.Add(v);
                    tokens.Add("(");
                    int cb = exp.IndexOf(')', exp.LastIndexOf('(', i));
                    if(cb == -1) cb = exp.Length;
                    tokens.AddRange(Tokenize(exp[(i + 1)..cb]));
                    v = "";
                    if(cb == exp.Length) break;
                    i = cb - 1;
                } else if(c == ')') {
                    tokens.Add(")");
                    v = "";
                    continue;
                }

                if(isNumber(i)) {
                    v += c;
                    mode = 0;
                } else {
                    if(mode == 0) {
                        if(!string.IsNullOrEmpty(v)) tokens.Add(v);
                        v = c.ToString();
                        mode = 1;
                    }

                    if(mode == 1) {
                        if(IsOpCode(v)) {
                            tokens.Add(v);
                            v = "";
                        } else {
                            if(IsOpCode(c.ToString())) {
                                if(!string.IsNullOrEmpty(v)) tokens.Add(v);
                                tokens.Add(c.ToString());
                                v = "";
                                mode = 1;
                            } else {
                                v += c;
                            }
                        }
                    }
                }
            }

            if(v != "") tokens.Add(v);

            return tokens;
        }

        // https://rosettacode.org/wiki/Parsing/RPN_to_infix_conversion#C.23
        public string RPNToInfix(string tokens) {
            Stack<string> stack = new();
            Stack<OpCode> ocs = new();

            foreach(string token in tokens.Split(' ')) {
                OpCode oc = GetOpCode(token);
                if(oc == null) {
                    stack.Push(token);
                } else {
                    string arg = "";
                    switch(oc.ArgumentCount) {
                        case 1:
                            switch(oc.Associativity) {
                                case Associativities.Left:
                                    arg = $"({stack.Pop()}){token}";
                                    break;
                                case Associativities.Right:
                                    arg = $"{token}({stack.Pop()})";
                                    break;
                            }
                            break;
                        case 2:
                            string v1 = stack.Pop();
                            string v2 = stack.Pop();
                            if(ocs.Count > 0 && oc.ComparePrecedence(ocs.Peek()) > 0) {
                                if(InferType(v1) == Types.Infix && ContainsOpCodes(Tokenize(v1))) v1 = $"({v1})";
                                if(InferType(v2) == Types.Infix && ContainsOpCodes(Tokenize(v2))) v2 = $"({v2})";
                            }
                            arg = $"{v2}{token}{v1}";
                            break;
                        default:
                            Debugger.Break(); // TODO: Convert this to a function of the form
                                              // function(a, b, c, ...)
                            break;
                    }

                    ocs.Push(oc);
                    stack.Push(arg);
                }
            }

            return stack.Pop();
        }
    }
}
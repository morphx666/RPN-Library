﻿using RPN.OpCodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static RPN.OpCodes.OpCode;

// HP48GX Manual: http://h10032.www1.hp.com/ctg/Manual/c00442262.pdf

namespace RPN {
    public class RPNStack {
        private readonly Stack<StackItem> stack;
        private readonly List<OpCode> opCodes;
        private readonly List<Variable> variables;
        private readonly bool simplifyTokens;

        public record StackItem {
            public string Token { get; init; }
            public Types Type { get; init; }
            public bool HasDelimiters { get; init; }

            public string AsString() {
                return Type switch {
                    Types.Infix => $"'{Token}'",
                    Types.String => $"\"{Token}\"".Replace("\t", " "),
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
                HasDelimiters = type == Types.Infix || type == Types.String;
            }
        }

        public class Variable {
            public string Name { get; set; }
            public string Value { get; set; }
            public Types Type { get; set; }

            public Variable(string name, string value, Types type) {
                Name = name;
                Value = value;
                Type = type;
            }
        }

        public List<Variable> Variables {
            get => variables;
        }

        public int ColumnWidth { get; init; }

        public Stack<StackItem> Stack {
            get => stack;
        }

        public List<OpCode> OpCodes {
            get => opCodes;
        }

        public string ErrorFunction { get; internal set; } = "";
        public string ErrorMessage { get; internal set; } = "";

        public RPNStack(int columnWidth = 22, bool simplifyTokens = true) {
            stack = new();
            opCodes = GetAvailableOpCodes();
            variables = new();
            ColumnWidth = columnWidth;
            this.simplifyTokens = simplifyTokens;
        }

        public void PrintStack(int max = 4, bool showTypes = false) {
            StackItem[] stk = new StackItem[max];
            Array.Copy(stack.ToArray(), 0, stk, 0, Math.Min(max, stack.Count));

            string tmp;
            string idx;
            for(int i = max - 1; i >= 0; i--) {
                idx = (i + 1).ToString();
                tmp = stk[i] == null ? "" : tmp = stk[i].AsString();
                tmp = tmp.Length <= ColumnWidth - (idx.Length + 1) ?
                        tmp :
                        "…" + tmp[(tmp.Length - ColumnWidth + idx.Length + 2)..];

                if(tmp != "" && stk[i].Type == Types.Infix) {
                    foreach(OpCode oc in opCodes.Where(o => o.SpaceArguments)) {
                        string s = oc.Symbols[0];
                        if(tmp.Contains(s)) {
                            int p = tmp.IndexOf(s);
                            tmp = $"{tmp[0..p]} {s} {tmp[(p + s.Length)..]}";
                            break;
                        }
                    }
                }

                Console.Write($"{tmp.PadLeft(ColumnWidth)}");
                if(showTypes) {
                    if(tmp != "") tmp = stk[i].Type.ToString();
                    Console.Write($"|{tmp,10}");
                }
                Console.CursorLeft = 0;
                Console.WriteLine(idx + ":");
            }
        }

        public void PrintVariables() {
            for(int i = 0; i < 6; i++) {
                Console.CursorLeft = i * 6;
                if(i < variables.Count) {
                    string n = variables[i].Name.Length <= 5 ? variables[i].Name : variables[i].Name[..5];
                    int l = (5 - n.Length) / 2;
                    Console.Write($"{"".PadLeft(l)}{n}{"".PadRight(5 - l - n.Length)}");
                } else {
                    Console.Write("     ");
                }
            }
        }

        public (int, int, string) PrintOpCodes(int sectionIndex, int opCodeIndex) {
            ConsoleColor bc = Console.BackgroundColor;

            string selectedOpCode = "";
            int sectionsCount = (from oc in opCodes select oc.Section).Distinct().Count();
            if(sectionIndex >= sectionsCount) sectionIndex = 0;
            if(sectionIndex < 0) sectionIndex = sectionsCount - 1;

            string sectionName = (from oc in opCodes
                                  orderby oc.Section
                                  select oc.Section)
                                  .Distinct()
                                  .ToArray()[sectionIndex];

            var socs = (from oc in opCodes
                           where oc.Section == sectionName
                           orderby oc.Symbols[0]
                           select oc.Symbols[0]).ToList();

            if(opCodeIndex >= socs.Count) opCodeIndex = 0;
            if(opCodeIndex < 0) opCodeIndex = socs.Count - 1;

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{sectionName,-35}\n");

            for(int i = 0; i < socs.Count; i++) {
               string n = socs[i].Length <= 5 ? socs[i] : socs[i][..5];
                int l = (5 - n.Length) / 2;

                if(i == opCodeIndex) {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    selectedOpCode = socs[i];
                } else {
                    Console.BackgroundColor = bc;
                }
                Console.Write($"{"".PadLeft(l)}{n}{"".PadRight(5 - l - n.Length)}");

                if(Console.CursorLeft >= Console.WindowWidth - 1 - 7) {
                    if(Console.CursorTop >= Console.WindowHeight - 1) break;
                    Console.CursorTop++;
                    Console.CursorLeft = 0;
                } else {
                    Console.CursorLeft++;
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            while(Console.CursorTop < Console.WindowHeight - 1) {
                Console.Write($"{"".PadRight(Console.WindowWidth - Console.CursorLeft)}");
                Console.CursorLeft = 0;
                Console.CursorTop++;
            }
            Console.BackgroundColor = bc;

            return (sectionIndex, opCodeIndex, selectedOpCode);
        }

        public bool IsOpCode(string token) {
            foreach(OpCode oc in opCodes) {
                if(oc.Symbols.Contains(token)) return true;
            }

            return false;
        }

        public OpCode GetOpCode(string token) {
            foreach(OpCode oc in opCodes) {
                if(oc.Symbols.Contains(token)) return oc;
            }

            return null;
        }

        private bool ContainsOpCodes(List<string> tokens, int minArgumentCount = 1) {
            foreach(string token in tokens) {
                OpCode oc = GetOpCode(token);
                if(oc != null) {
                    if(minArgumentCount > 1) return oc.ArgumentCount >= minArgumentCount;
                    return true;
                }
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

        public bool Push(string tokens, Types dataType = Types.Any, bool expand = false) {
            ResetErrorState();
            if(tokens == "") return false;

            // Remove spaces from infix expressions
            string fixedTokens = "";
            bool isInfix = false;
            bool isString = false;
            for(int i = 0; i < tokens.Length; i++) {
                switch(tokens[i]) {
                    case '\'':
                        isInfix = !isInfix && !isString;
                        fixedTokens += '\'';
                        break;
                    case '"':
                        isString = !isString;
                        fixedTokens += '"';
                        break;
                    case ' ':
                        if(isString)
                            fixedTokens += '\t';
                        else if(!isInfix) fixedTokens += ' ';
                        break;
                    default:
                        fixedTokens += tokens[i];
                        break;
                }
            }

            bool isFunction = false;
            foreach(string token in fixedTokens.Split(' ')) {
                isFunction |= ParseToken(token, dataType, expand);
            }

            return isFunction;
        }

        private bool ParseToken(string token, Types dataType = Types.Any, bool expand = false) {
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
                            foreach(Variable v in variables) {
                                if(v.Name == token) {
                                    stack.Push(new StackItem(v.Value, v.Type));
                                    return false;
                                }
                            }

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
            if(!simplifyTokens) return token;

            token = token.Replace("'", "");
            string infix = InfixToRPN(token);
            if(infix == token) {
                return token;
            } else {
                return RPNToInfix(infix);
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

        // This function has the fatal flaw that it won't recognize opcodes with symbols longer than 1 char
        // I guess the solution would be to store each opcode as a unique byte value
        // Similar solution to how the support for spaces was implemented in commit aec71976
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
                    if(!string.IsNullOrEmpty(v) && v != "(") tokens.Add(v);
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
        public string RPNToInfix(string tokens) { // FIXME: This code sucks!
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
                            bool v1hoc = ContainsOpCodes(Tokenize(v1), 2);
                            bool v2hoc = ContainsOpCodes(Tokenize(v2), 2);

                            //if(v1hoc) v1hoc &= GetOpCode(v1).ArgumentCount > 1;
                            //if(v2hoc) v2hoc &= GetOpCode(v2).ArgumentCount > 1;

                            if(ocs.Count > 0 && (oc.ComparePrecedence(ocs.Peek()) > 0 || v1hoc || v2hoc)) {
                                if(InferType(v1) == Types.Infix && v1hoc) v1 = $"({v1})";
                                if(InferType(v2) == Types.Infix && v2hoc) v2 = $"({v2})";
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
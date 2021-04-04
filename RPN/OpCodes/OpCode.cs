using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPN.OpCodes {
    public abstract class OpCode {
        public enum Types {
            Unknown = 0,

            Integer = 1,
            Float = 2,
            Complex = 4,

            Real = Integer | Float,
            Number = Real | Complex,

            Formula = 8,

            String = 16,

            Any = 31
        }

        public string ErrorFunction { get; internal set; } = "";
        public string ErrorMessage { get; internal set; } = "";
        public int ArgumentCount { get; init; }
        public string[] Symbols { get; init; }
        public Types[] DataTypes { get; init; }
        public abstract void ExecuteInternal(Stack<string> stack, Types dataType);

        public bool Execute(Stack<string> stack) {
            try {
                if(stack.Count < ArgumentCount) {
                    throw new Exception($"Too Few Arguments");
                }

                int dataType = 0;
                if(ArgumentCount > 0) {
                    string[] tokens = new string[ArgumentCount];
                    Array.Copy(stack.ToArray(), 0, tokens, 0, ArgumentCount);

                    for(int i = 0; i < tokens.Length; i++) {
                        for(int j = 0; j < DataTypes.Length; j++) {
                            if((int)(InferType(tokens[i]) & DataTypes[j]) != 0) {
                                dataType = Math.Max((int)dataType, (int)DataTypes[j]);
                            }
                        }
                    }

                    if(dataType == 0) throw new Exception("Bad argument type");
                }

                ExecuteInternal(stack, (Types)dataType);
                return true;
            } catch(Exception ex) {  // FIXME: This is kind of pointless.
                                     // We should be able to handle errors such as:
                                     // - To few arguments (already implemented)
                                     // - Invalid argument types
                                     // - Overflow / Division by Zero / etc...

                ErrorFunction = Symbols[0] + " Error:";
                ErrorMessage = ex.Message;
                return false;
            }
        }

        internal static Types InferType(string token) {
            if(double.TryParse(token, out double v)) {
                if(Math.Floor(v) == v) return Types.Integer;
                return Types.Float;
            } else if(token.StartsWith('(')) {
                return Types.Complex;
            } else if(token.StartsWith('"')) {
                return Types.String;
            } else
                return Types.Formula;
        }

        public static List<OpCode> GetAvailableFunctions() {
            List<OpCode> functions = new();

            Type functionType = typeof(OpCode);
            Assembly asm = Assembly.GetExecutingAssembly();
            Type fa = typeof(OpCodeAttr);

            foreach(Type t in asm.GetTypes())
                if(t.GetCustomAttribute(fa) != null)
                    functions.Add((OpCode)(t.Assembly.CreateInstance(t.FullName)));

            return functions;
        }
    }
}
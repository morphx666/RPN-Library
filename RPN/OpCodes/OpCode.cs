using System;
using System.Collections.Generic;
using System.Reflection;
using static RPN.RPNStack;

namespace RPN.OpCodes {
    public abstract class OpCode {
        public enum Types {
            Unknown = 0,

            Integer = 1,
            Float = 2,
            Complex = 4,

            Real = Integer | Float,
            Number = Real | Complex,

            Infix = 8,

            String = 16,

            OpCode = 32,

            Any = 63
        }

        public enum Associativities {
            Left,
            Right
        }

        public string ErrorFunction { get; internal set; } = "";
        public string ErrorMessage { get; internal set; } = "";
        public int ArgumentCount { get; init; } = 0;
        public string[] Symbols { get; init; }
        public Types[] DataTypes { get; init; } = { Types.Any };
        public Associativities Associativity { get; init; }
        public int Precedence { get; init; } = 10;
        public abstract void ExecuteInternal(RPNStack rpn, Types dataType);

        public bool Execute(RPNStack rpn) {
            try {
                if(rpn.Count < ArgumentCount) {
                    throw new Exception($"Too Few Arguments");
                }

                int dataType = 0;
                if(ArgumentCount > 0) {
                    StackItem[] tokens = new StackItem[ArgumentCount];
                    Array.Copy(rpn.ToArray(), 0, tokens, 0, ArgumentCount);

                    for(int i = 0; i < tokens.Length; i++) {
                        for(int j = 0; j < DataTypes.Length; j++) {
                            if((tokens[i].Type & DataTypes[j]) != 0) {
                                //dataType = Math.Max(dataType, (int)DataTypes[j]);
                                dataType = Math.Max(dataType, (int)tokens[i].Type);
                            }
                        }
                    }

                    if(dataType == 0) {
                        throw new Exception("Bad Argument Type");
                    }
                }

                ExecuteInternal(rpn, (Types)dataType);
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
            if(token == "") {
                return Types.Any;
            } else { 
                if(double.TryParse(token, out double v)) {
                    if((int)v == v) return Types.Integer;
                    return Types.Float;
                } else if(token.StartsWith('(') && token.Contains(',')) {
                    return Types.Complex;
                } else if(token.StartsWith('"')) {
                    return Types.String;
                } else
                    return Types.Infix;
            }
        }

        public static List<OpCode> GetAvailableOpCodes() {
            List<OpCode> opCodes = new();

            Assembly asm = Assembly.GetExecutingAssembly();
            Type fa = typeof(OpCodeAttr);

            foreach(Type t in asm.GetTypes()) {
                if(t.GetCustomAttribute(fa) != null) {
                    opCodes.Add((OpCode)(t.Assembly.CreateInstance(t.FullName)));
                }
            }

            return opCodes;
        }

        public int ComparePrecedence(OpCode oc) {
            return this.Precedence - oc.Precedence;
        }
    }
}
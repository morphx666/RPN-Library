using System;
using System.Collections.Generic;
using RPN.OpCodes.Special;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Comb))]
    public class Comb : OpCode {
        public Comb() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Comb).ToUpper() };
            base.DataTypes = new Types[] { Types.Number, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                string v2 = stack.Pop().Replace("'", "");
                stack.Push($"'{Symbols[0]}({v2},{v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                double v2 = double.Parse(stack.Pop());
                if(v2 < v1) {
                    stack.Push(v2.ToString());
                    stack.Push(v1.ToString());
                    throw new ArgumentException("Bad argument value");
                }
                stack.Push((SpecialFunctions.Fact(v2) / (SpecialFunctions.Fact(v1) * SpecialFunctions.Fact(v2 - v1))).ToString());
            }
        }
    }
}
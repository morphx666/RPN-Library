using System;
using RPN.OpCodes.Special;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Perm))]
    public class Perm : OpCode {
        public Perm() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Perm).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop().Replace("'", "");
                string v2 = rpn.Pop().Replace("'", "");
                rpn.Push($"'{Symbols[0]}({v2},{v1})'");
            } else {
                double v1 = double.Parse(rpn.Pop());
                double v2 = double.Parse(rpn.Pop());
                if(v2 < v1) {
                    rpn.Push(v2.ToString());
                    rpn.Push(v1.ToString());
                    throw new ArgumentException("Bad argument value");
                }
                rpn.Push((SpecialFunctions.Fact(v2) / SpecialFunctions.Fact(v2 - v1)).ToString());
            }
        }
    }
}
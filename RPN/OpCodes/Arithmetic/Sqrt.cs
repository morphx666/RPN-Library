using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Sqrt))]
    public class Sqrt : OpCode {
        public Sqrt() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Sqrt).ToUpper(), "√" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 5;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop().Replace("'", "");
                rpn.Push($"'√({v1})'");
            } else {
                double v1 = double.Parse(rpn.Pop());
                rpn.Push(Math.Sqrt(v1).ToString());
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Trigonometry {
    [OpCodeAttr(nameof(Tan))]
    public class Tan : OpCode {
        public Tan() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Tan).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop().Replace("'", "");
                rpn.Push($"'{Symbols[0]}({v1})'");
            } else {
                double v1 = double.Parse(rpn.Pop());
                rpn.Push(Math.Tan(v1).ToString());
            }
        }
    }
}
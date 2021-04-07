using System;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Pow))]
    public class Pow : OpCode {
        public Pow() {
            ArgumentCount = 2;
            Symbols = new string[] { "^" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 5;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop().Replace("'", "");
                string v2 = rpn.Pop().Replace("'", "");
                rpn.Push($"'({v2})^({v1})'");
            } else {
                double v1 = double.Parse(rpn.Pop());
                double v2 = double.Parse(rpn.Pop());
                rpn.Push(Math.Pow(v2, v1).ToString());
            }
        }
    }
}
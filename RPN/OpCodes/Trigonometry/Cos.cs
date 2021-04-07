using System;

namespace RPN.OpCodes.Trigonometry {
    [OpCodeAttr(nameof(Cos))]
    public class Cos : OpCode {
        public Cos() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Cos).ToUpper() };
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
                rpn.Push(Math.Cos(v1).ToString());
            }
        }
    }
}
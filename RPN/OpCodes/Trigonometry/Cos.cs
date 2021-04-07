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
            string v1 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"{Symbols[0]}({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                rpn.Push(Math.Cos(d1).ToString(), dataType);
            }
        }
    }
}
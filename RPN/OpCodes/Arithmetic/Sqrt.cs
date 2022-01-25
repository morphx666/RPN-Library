using System;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Sqrt))]
    public class Sqrt : OpCode {
        public Sqrt() {
            ArgumentCount = 1;
            Symbols = new string[] { "√", nameof(Sqrt).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 5;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"{Symbols[0]}({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                rpn.Push(Math.Sqrt(d1).ToString(), dataType);
            }

            return true;
        }
    }
}
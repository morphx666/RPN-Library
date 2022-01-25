using System;

namespace RPN.OpCodes.Trigonometry {
    [OpCodeAttr(nameof(Sin))]
    public class Sin : OpCode {
        public Sin() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Sin).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"{Symbols[0]}({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                rpn.Push(Math.Sin(d1).ToString(), dataType);
            }

            return true;
        }
    }
}
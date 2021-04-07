using System;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(XRoot))]
    public class XRoot : OpCode {
        public XRoot() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(XRoot).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 5;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"({v2})^({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                double d2 = double.Parse(v2);
                rpn.Push(Math.Pow(d2, 1/d1).ToString(), dataType);
            }
        }
    }
}
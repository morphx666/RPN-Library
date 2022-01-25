namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Add))]
    public class Add : OpCode {
        public Add() {
            ArgumentCount = 2;
            Symbols = new string[] { "+" };
            DataTypes = new Types[] { Types.Number, Types.String, Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 0;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;

            if((dataType & Types.String) == Types.String) {
                rpn.Push($"{v2}{v1}", dataType);
            } else if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"({v2}){Symbols[0]}({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                double d2 = double.Parse(v2);
                rpn.Push((d2 + d1).ToString(), dataType);
            }

            return true;
        }
    }
}
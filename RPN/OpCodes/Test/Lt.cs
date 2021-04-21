namespace RPN.OpCodes.Test {
    [OpCodeAttr(nameof(Lt))]
    public class Lt : OpCode {
        public Lt() {
            ArgumentCount = 2;
            Symbols = new string[] { "<" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 5;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"({v2}){Symbols[0]}({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                double d2 = double.Parse(v2);
                rpn.Push(d2 < d1 ? "-1" : "0", dataType);
            }
        }
    }
}
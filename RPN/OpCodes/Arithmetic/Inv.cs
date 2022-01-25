namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Inv))]
    public class Inv : OpCode {
        public Inv() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Inv).ToUpper() };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 5;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"1/({v1})", dataType);
            } else {
                double d1 = double.Parse(v1);
                rpn.Push((1 / d1).ToString(), dataType);
            }

            return true;
        }
    }
}
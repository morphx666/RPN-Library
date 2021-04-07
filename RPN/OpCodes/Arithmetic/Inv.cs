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

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop();
                rpn.Push($"1/({v1})");
            } else {
                double v1 = double.Parse(rpn.Pop());
                rpn.Push((1 / v1).ToString());
            }
        }
    }
}
namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Div))]
    public class Div : OpCode {
        public Div() {
            ArgumentCount = 2;
            Symbols = new string[] { "/", "÷" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 5;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop();
                string v2 = rpn.Pop();
                rpn.Push($"({v2})/({v1})");
            } else {
                double v1 = double.Parse(rpn.Pop());
                double v2 = double.Parse(rpn.Pop());
                rpn.Push((v2 / v1).ToString());
            }
        }
    }
}
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

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.String) == Types.String) {
                string v1 = rpn.Pop().Replace("\"", "");
                string v2 = rpn.Pop().Replace("\"", "");
                rpn.Push($"\"{v2}{v1}\"");
            } else if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop();
                string v2 = rpn.Pop();
                rpn.Push($"({v2})+({v1})");
            } else {
                double v1 = double.Parse(rpn.Pop());
                double v2 = double.Parse(rpn.Pop());
                rpn.Push((v2 + v1).ToString());
            }
        }
    }
}
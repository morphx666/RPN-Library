namespace RPN.OpCodes.Misc {
    [OpCodeAttr(nameof(Eval))]
    public class Eval : OpCode {
        public Eval() {
            ArgumentCount = 1;
            Symbols = new string[] { "EVAL" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 5;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            foreach(string token in rpn.InfixToRPN(v1).Split(' ')) {
                rpn.Push(token, Types.Any, true);
            }

            return true;
        }
    }
}
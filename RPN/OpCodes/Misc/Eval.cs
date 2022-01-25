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

            if(rpn.Count == 0) {
                ErrorFunction = "EVAL";
                ErrorMessage = "Too Few Arguments";
                return false;
            }

            foreach(string token in rpn.InfixToRPN(v1).Split(' ')) {
                rpn.Push(token);
            }

            return true;
        }
    }
}
namespace RPN.OpCodes.Memory {
    [OpCodeAttr(nameof(Purge))]
    internal class Purge : OpCode {
        public Purge() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Purge).ToUpper() };
            DataTypes = new Types[] { Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 5;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;

            foreach(RPNStack.Variable v in rpn.Variables) {
                if(v.Name == v1) {
                    rpn.Variables.Remove(v);
                    break;
                }
            }

            return true;
        }
    }
}
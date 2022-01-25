namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Dup2))]
    public class Dup2 : OpCode {
        public Dup2() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Dup2).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            rpn.Push("2 DUPN");

            return true;
        }
    }
}
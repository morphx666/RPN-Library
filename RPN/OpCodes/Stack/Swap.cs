namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Swap))]
    public class Swap : OpCode {
        public Swap() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Swap).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;
            rpn.Push(v1);
            rpn.Push(v2);

            return true;
        }
    }
}
namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Swap))]
    public class Swap : OpCode {
        public Swap() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Swap).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop();
            string v2 = rpn.Pop();
            rpn.Push(v1);
            rpn.Push(v2);
        }
    }
}
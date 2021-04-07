namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Dup))]
    public class Dup : OpCode {
        public Dup() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Dup).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            rpn.Push(rpn.Peek());
        }
    }
}
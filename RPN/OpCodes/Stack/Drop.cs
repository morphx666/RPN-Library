namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Drop))]
    public class Drop : OpCode {
        public Drop() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Drop).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            rpn.Pop();

            return true;
        }
    }
}
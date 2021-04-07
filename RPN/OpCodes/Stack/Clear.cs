namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Clear))]
    public class Clear : OpCode {
        public Clear() {
            Symbols = new string[] { nameof(Clear).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            rpn.Clear();
        }
    }
}
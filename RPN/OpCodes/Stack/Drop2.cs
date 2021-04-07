namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Drop2))]
    public class Drop2 : OpCode {
        public Drop2() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Drop2).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            rpn.Push("2 DROPN");
        }
    }
}
namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Over))]
    public class Over : OpCode {
        public Over() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Over).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;
            rpn.Push(v2);
            rpn.Push(v1);
            rpn.Push(v2);

            return true;
        }
    }
}
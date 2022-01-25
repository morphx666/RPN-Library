namespace RPN.OpCodes.Logic {
    [OpCodeAttr(nameof(BNot))]
    public class BNot : OpCode {
        public BNot() {
            ArgumentCount = 1;
            Symbols = new string[] { "NOT" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 5;
            SpaceArguments = true;
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"{Symbols[0]}({v1})", dataType);
            //} else if((dataType & Types.Binary) == Types.Binary) { // Not yet supported
            //    int d1 = (int)double.Parse(v1);
            //    rpn.Push((!d1).ToString(), dataType);
            } else {
                int d1 = (int)double.Parse(v1);
                rpn.Push(d1 == 0 ? "1" : "0", dataType);
            }

            return true;
        }
    }
}
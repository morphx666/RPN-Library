namespace RPN.OpCodes.Logic {
    [OpCodeAttr(nameof(BAnd))]
    public class BAnd : OpCode {
        public BAnd() {
            ArgumentCount = 2;
            Symbols = new string[] { "AND" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Left;
            Precedence = 5;
            SpaceArguments = true;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            string v1 = rpn.Pop().Token;
            string v2 = rpn.Pop().Token;

            if((dataType & Types.Infix) == Types.Infix) {
                rpn.Push($"({v2}){Symbols[0]}({v1})", dataType);
            //} else if((dataType & Types.Binary) == Types.Binary) { // Not yet supported
            //    int d1 = (int)double.Parse(v1);
            //    int d2 = (int)double.Parse(v2);
            //    rpn.Push((d2 & d1).ToString(), dataType);
            } else {
                int d1 = (int)double.Parse(v1);
                int d2 = (int)double.Parse(v2);
                rpn.Push(d2 != 0 && d1 != 0 ? "1" : "0", dataType);
            }
        }
    }
}
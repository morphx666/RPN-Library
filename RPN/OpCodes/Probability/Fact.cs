using RPN.OpCodes.Special;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Fact))]
    public class Fact : OpCode {
        public Fact() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(Fact).ToUpper(), "!" };
            DataTypes = new Types[] { Types.Number, Types.Infix };
            Associativity = Associativities.Right;
            Precedence = 10;
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            if((dataType & Types.Infix) == Types.Infix) {
                string v1 = rpn.Pop();
                rpn.Push($"{Symbols[0]}({v1})");
            } else {
                double v1 = double.Parse(rpn.Pop());
                rpn.Push(SpecialFunctions.Fact(v1).ToString());
            }
        }
    }
}
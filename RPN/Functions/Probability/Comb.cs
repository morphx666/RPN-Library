using System;
using System.Collections.Generic;
using RPN.Functions.Special;

namespace RPN.Functions.Probability
{
    [RPNFunctionAttr(nameof(Comb))]
    public class Comb : Function
    {
        public Comb()
        {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Comb).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack)
        {
            double v1 = double.Parse(stack.Pop());
            double v2 = double.Parse(stack.Pop());
            if (v2 < v1) {
                stack.Push(v2.ToString());
                stack.Push(v1.ToString());
                throw new ArgumentException("Bad argument value");
            }
            stack.Push((SpecialFunctions.Fact(v2) / (SpecialFunctions.Fact(v1) * SpecialFunctions.Fact(v2 - v1))).ToString());
        }
    }
}
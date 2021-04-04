using System;

namespace RPN {
    [AttributeUsage(AttributeTargets.Class)]
    public class OpCodeAttr : Attribute {
        private string Description { get; init; }

        public OpCodeAttr(string description) {
            Description = description;
        }
    }
}
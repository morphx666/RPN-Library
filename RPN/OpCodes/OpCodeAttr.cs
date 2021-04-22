using System;

namespace RPN {
    [AttributeUsage(AttributeTargets.Class)]
    public class OpCodeAttr : Attribute {
        public string Description { get; init; }

        public OpCodeAttr(string description) {
            Description = description;
        }
    }
}
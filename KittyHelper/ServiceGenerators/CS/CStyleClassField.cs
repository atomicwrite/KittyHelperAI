using System;
using System.Collections.Generic;
using System.Linq;
using KittyHelper.ServiceGenerators.CS;
using ServiceStack.Text;

namespace KittyHelper
{
    public class CStyleClassField : CStyleStatement
    {
        protected readonly string Name;
        protected readonly CStyleTypeDeclaration Type;
        private readonly string accessor;
        private readonly bool getterSetter;
        private readonly CStyleLambda getter;
        private readonly CStyleLambda setter;
        private readonly string defaultValue;
        protected readonly List<CStyleDecorator> Decorators;

        public CStyleClassField(string name, CStyleTypeDeclaration type, string defaultValue = "",
            string accessor = "public", bool getterSetter = true, CStyleLambda cStyleLambdaGetter = null,
            CStyleLambda cStyleLambdaSetter = null, params CStyleDecorator[] decorators)
        {
            this.getter = cStyleLambdaGetter ??= CStyleLambda.Get;
            this.setter = cStyleLambdaSetter ??= CStyleLambda.Set;
            this.Decorators = new List<CStyleDecorator>(decorators);
            this.Name = name;
            this.Type = type;
            this.accessor = accessor;
            this.getterSetter = getterSetter;

            this.defaultValue = defaultValue;
        }

        public override string Render()
        {
            string dValue = "";
            var hasDefaultValue = !string.IsNullOrEmpty(defaultValue);
            if (hasDefaultValue)
            {
                dValue = " = " + defaultValue ;
            }

            string getterSteterStr = "";
            if (getterSetter)
            {
                var renderGetter = getter != null ? getter.Render() : "";
                var renderSetter = setter != null ? setter.Render() : "";
                getterSteterStr = $"{{ {renderGetter} {renderSetter} }}";
            }

            var semi = "";
            if (hasDefaultValue || !getterSetter)
            {
                semi = ";";
            }

            string decoratorString = string.Join(Environment.NewLine, Decorators.Select(a => a.Render()));
            return @$"
                                {decoratorString}
                                {accessor} {Type.Render()} {Name}  {getterSteterStr} {dValue} {semi}
";
        }
    }
}
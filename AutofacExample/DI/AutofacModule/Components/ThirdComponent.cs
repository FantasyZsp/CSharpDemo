using System;

namespace AutofacExample.DI.AutofacModule.Components
{
    public class ThirdComponent : INumericComponent
    {
        private readonly FirstComponent _firstComponent;
        private readonly SecondComponent _secondComponent;


        public ThirdComponent(FirstComponent firstComponent, SecondComponent secondComponent)
        {
            _firstComponent = firstComponent;
            _secondComponent = secondComponent;
            Console.WriteLine($"ThirdComponent with {_firstComponent.GetHashCode()} and {_secondComponent.GetHashCode()}");
        }


        public void Produce()
        {
            Console.WriteLine(GetType());
        }
    }
}
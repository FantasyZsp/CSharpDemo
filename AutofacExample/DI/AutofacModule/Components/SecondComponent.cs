using System;

namespace AutofacExample.DI.AutofacModule.Components
{
    public class SecondComponent : INumericComponent
    {
        public SecondComponent()
        {
            Console.WriteLine($"SecondComponent with {GetHashCode()}");
        }

        public void Produce()
        {
            Console.WriteLine(GetType());
        }
    }
}
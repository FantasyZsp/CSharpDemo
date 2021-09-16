using System;

namespace AutofacExample.DI.AutofacModule.Components
{
    public class FirstComponent : INumericComponent
    {
        public FirstComponent()
        {
            Console.WriteLine($"FirstComponent with {GetHashCode()}");
        }

        public void Produce()
        {
            Console.WriteLine(GetType());
        }
    }
}
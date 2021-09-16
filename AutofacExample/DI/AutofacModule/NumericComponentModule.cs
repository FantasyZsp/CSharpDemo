using System;
using Autofac;
using AutofacExample.DI.AutofacModule.Components;

namespace AutofacExample.DI.AutofacModule
{
    public class NumericComponentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FirstComponent>().SingleInstance()
                .OnPreparing(args => { Console.WriteLine($"FirstComponent OnPreparing with {args}"); })
                .OnActivating(args => { Console.WriteLine($"FirstComponent OnActivating with {args}"); })
                .OnActivated(args => { Console.WriteLine($"FirstComponent OnActivated with {args}"); })
                ;
            builder.RegisterType<SecondComponent>().SingleInstance()
                .OnPreparing(args => { Console.WriteLine($"SecondComponent OnPreparing with {args}"); })
                .OnActivating(args => { Console.WriteLine($"SecondComponent OnActivating with {args}"); })
                .OnActivated(args => { Console.WriteLine($"SecondComponent OnActivated with {args}"); });
            builder.RegisterType<ThirdComponent>().SingleInstance()
                .OnPreparing(args => { Console.WriteLine($"ThirdComponent OnPreparing with {args}"); })
                .OnActivating(args => { Console.WriteLine($"ThirdComponent OnActivating with {args}"); })
                .OnActivated(args => { Console.WriteLine($"ThirdComponent OnActivated with {args}"); });
        }
    }
}
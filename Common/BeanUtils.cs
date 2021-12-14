using System;

namespace Common
{
    public static class BeanUtils
    {
        public static T Copy<T>(object model)
        {
            var result = Activator.CreateInstance<T>();
            foreach (var info in typeof(T).GetProperties())
            {
                var pro = model.GetType().GetProperty(info.Name);
                if (pro != null)
                    info.SetValue(result, pro.GetValue(model));
            }

            return result;
        }
    }
}
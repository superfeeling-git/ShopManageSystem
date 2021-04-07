using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shop.Utility
{
    public static class ProperyHelper
    {
        public static string GetKeyName<T>()
        {
            //获取类型
            Type type = typeof(T);

            //获取主键属性
            PropertyInfo propertyInfo = type.GetProperties().Where(m => m.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();

            //返回属性名称
            return propertyInfo.Name;
        }

        public static object GetKeyValue(this object Tentity)
        {
            //获取类型
            Type type = Tentity.GetType();

            //获取主键属性
            PropertyInfo propertyInfo = type.GetProperties().Where(m => m.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();

            //返回属性名称
            return propertyInfo.GetValue(Tentity);
        }

        public static int GetKeyValueForInt(this object Tentity)
        {
            return Convert.ToInt32(GetKeyValue(Tentity));
        }
    }
}

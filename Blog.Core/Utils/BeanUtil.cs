using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Exceptions;

namespace Blog.Core.Utils
{
    public class BeanUtil
    {
        /// <summary>
        /// 检查实体类对象中指定字段是否存在或为 null
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="entity">实体类对象实例</param>
        /// <param name="fieldName">要检查的字段名（字符串）</param>
        /// <param name="inputString">您提到的“一段字符”，这里假设是用于特定判断或输出的额外信息</param>
        /// <returns>
        /// 如果字段不存在或值为 null，返回 true；
        /// 如果字段存在且值不为 null，返回 false。
        /// </returns>
        public static void IsFieldNullOrMissing<T>(T entity, string fieldName,string columnName = "")
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "实体对象不能为 null");
            }
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new BusinessException("字段名不能为空", nameof(fieldName));
            }

            Type type = typeof(T);

            // 1. 检查是否存在名为 fieldName 的属性
            PropertyInfo property = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                // 字段不存在
                throw new BusinessException($"警告：{columnName} 不存在。");
            }

            // 2. 获取该属性的值
            object value = property.GetValue(entity);

            // 3. 判断值是否为 null
            if (value == null)
            {
                throw new BusinessException($"警告：{columnName} 值为 null。");
            }
        }
    }
}

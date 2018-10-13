using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class EnumUtil
    {
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }
            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        public static T GetEnum<T>(string description)
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                object[] objAttrs = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(EnumDescriptionAttribute), true);
                if (objAttrs != null && objAttrs.Length > 0)
                {
                    EnumDescriptionAttribute descAttr = objAttrs[0] as EnumDescriptionAttribute;
                    if (descAttr.Description.Equals(description))
                    {
                        return (T)value;
                    }
                }
            }
            throw new ArgumentException("找不到该描述的枚举值");
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public string Description { get { return description; } }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }
}

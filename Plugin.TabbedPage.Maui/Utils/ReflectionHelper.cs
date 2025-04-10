using System.Reflection;

namespace Plugin.TabbedPage.Maui.Utils
{
    internal static class ReflectionHelper
    {

        public static T GetPropertyValue<T>(object obj, string propertyName)
        {
            return (T)GetPropertyValue(obj, propertyName);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var objType = obj.GetType();
            var propInfo = GetPropertyInfo(objType, propertyName);
            if (propInfo == null)
            {
                throw new ArgumentException(nameof(propertyName),
                    $"Couldn't find property {propertyName} in type {objType.FullName}");
            }

            return propInfo.GetValue(obj, null);
        }


        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propInfo;
            do
            {
                propInfo = type.GetProperty(propertyName,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic|
                    BindingFlags.IgnoreCase);

                type = type.BaseType;
            } while (propInfo == null && type != null);

            return propInfo;
        }

        public static T GetFieldValue<T>(object obj, string fieldName)
        {
            return (T)GetFieldValue(obj, fieldName);
        }

        public static object GetFieldValue(object obj, string fieldName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
            {
                throw new ArgumentException(nameof(fieldName),
                    $"Couldn't find field {fieldName} in type {objType.FullName}");
            }

            return fieldInfo.GetValue(obj);
        }

        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic|
                    BindingFlags.IgnoreCase);

                type = type.BaseType;
            } while (fieldInfo == null && type != null);

            return fieldInfo;
        }
    }
}
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ArchiAgile.Client.Util
{
    public class ModelValidator
    {
        public bool Validate(object instance, out List<ValidationResult> errors)
        {
            errors = new List<ValidationResult>();
            List<ValidationResult> res = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), errors, true);
            return isValid;
        }
        public bool ValidateRecursive(object instance, out List<ValidationResult> errors)
        {
            bool result = Validate(instance, out errors);
            var properties = instance.GetType().GetProperties().Where(prop => prop.CanRead
                            && prop.GetIndexParameters().Length == 0).ToList();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType) continue;

                var value = GetPropertyValue(instance, property.Name);

                if (value == null) continue;

                var asEnumerable = value as IEnumerable;
                if (asEnumerable != null)
                {
                    foreach (var enumObj in asEnumerable)
                    {
                        var nestedResults = new List<ValidationResult>();
                        if (!ValidateRecursive(enumObj, out nestedResults))
                        {
                            result = false;
                            foreach (var validationResult in nestedResults)
                            {
                                PropertyInfo property1 = property;
                                errors.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                            }
                        };
                    }
                }
                else
                {
                    var nestedResults = new List<ValidationResult>();
                    if (!ValidateRecursive(value, out nestedResults))
                    {
                        result = false;
                        foreach (var validationResult in nestedResults)
                        {
                            PropertyInfo property1 = property;
                            errors.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                        }
                    }
                }
            }

            return result;
        }

        public object GetPropertyValue(object o, string propertyName)
        {
            object objValue = string.Empty;

            var propertyInfo = o.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
                objValue = propertyInfo.GetValue(o, null);

            return objValue;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.mdm_attributes;



namespace trifenix.agro.validator.operations
{

    /// <summary>
    /// Valida campos con atributos definidos
    /// </summary>
    public class Validator : IValidator {

        private Dictionary<string, IValidate> Validators { get; }

        public Validator(Dictionary<string, IValidate> _validators) {
            Validators = _validators;
        }

        private static List<dynamic> CreateDynamicList(object Obj) => Obj is IList ? (Obj is Array ? ((Array)Obj).Cast<dynamic>().ToList() : new List<dynamic>((IEnumerable<dynamic>)Obj)) : new List<dynamic> { Obj };

        private static bool IsNonRecursive(object Obj) {
            if (Obj == null)
                return true;
            Type type = Obj is IList ? (Obj is Array ? Obj.GetType().GetElementType() : Obj.GetType().GetGenericArguments()[0]) : Obj.GetType();
            return type.IsPrimitive || Nullable.GetUnderlyingType(type) != null || type.Equals(typeof(string)) || type.Equals(typeof(DateTime));
        }


        public async Task ValidateRecursively<T_Attr>(object obj) where T_Attr : Attribute {
            var errors = new List<string>();
            object[] args = null;
            if (obj == null)
                errors.Add("Referencia invalida. Objeto no puede ser nulo!");
            else {
                int errorCount = 0;
                var validatorName = typeof(T_Attr).Name;

                if (!Validators.TryGetValue(validatorName, out IValidate validator))
                    throw new NotImplementedException($"No existe la implementacion de la interface IValidate con este nombre '{validatorName}'. ");


                var properties = obj.GetType().GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(BrowsableAttribute)));
                var properties_Attr = properties.Where(prop => Attribute.IsDefined(prop, typeof(T_Attr))).ToList();
                foreach (var prop in properties_Attr) {
                    var values = CreateDynamicList(prop.GetValue(obj));
                    foreach (var value in values) {
                        try {
                            if (typeof(ReferenceAttribute).IsAssignableFrom(typeof(T_Attr)))
                                args = new object[] { ((ReferenceAttribute)prop.GetCustomAttributes(typeof(ReferenceAttribute), true).FirstOrDefault()).entityOfReference };
                            else if(typeof(UniqueAttribute).IsAssignableFrom(typeof(T_Attr)))
                                args = new object[] { obj.GetType(), prop.Name, (string)obj.GetType().GetProperty("Id")?.GetValue(obj) };
                            await validator.Validate(value, args);
                        } catch (Validation_Exception v_ex) {
                            errorCount++;
                            errors.Add($"{prop.Name}: {v_ex.ErrorMessages.FirstOrDefault()}");
                        }
                    }
                }
                if (errorCount > 0)
                    errors.Insert(0, $"\n{obj.GetType().Name}:");
                else {
                    try
                    {
                        var lst = properties.Where(prop => {

                            try
                            {
                                var value = prop.GetValue(obj, null);

                                var isNonRecursive = !IsNonRecursive(value);
                                return isNonRecursive;
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        });
                        var deepProperties = lst.Select(prop => prop.GetValue(obj, null)).ToList();

                        deepProperties.ForEach(propValue =>
                        {
                            var values = CreateDynamicList(propValue);
                            values.ForEach(value =>
                            {
                                try
                                {
                                    ValidateRecursively<T_Attr>(value);
                                }
                                catch (Validation_Exception v_ex)
                                {
                                    errors.AddRange(v_ex.ErrorMessages);
                                }
                            });
                        });
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                }
            }
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

    }

}
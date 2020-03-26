using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.model.external.Input;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.validator.operations {
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
                var properties = obj.GetType().GetProperties();
                var properties_Attr = properties.Where(prop => Attribute.IsDefined(prop, typeof(T_Attr))).ToList();
                foreach (var prop in properties_Attr) {
                    var values = CreateDynamicList(prop.GetValue(obj));
                    foreach (var value in values) {
                        try {
                            if (typeof(Reference).IsAssignableFrom(typeof(T_Attr)))
                                args = new object[] { ((Reference)prop.GetCustomAttributes(typeof(Reference), true).FirstOrDefault()).entityOfReference };
                            else if(typeof(Unique).IsAssignableFrom(typeof(T_Attr)))
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
                    var deepProperties = properties.Where(prop => !IsNonRecursive(prop.GetValue(obj))).Select(prop => prop.GetValue(obj)).ToList();
                    deepProperties.ForEach(propValue => {
                        var values = CreateDynamicList(propValue);
                        values.ForEach(value => {
                            try { 
                                ValidateRecursively<T_Attr>(value);
                            }
                            catch (Validation_Exception v_ex) {
                                errors.AddRange(v_ex.ErrorMessages);
                            }
                        });
                    });
                }
            }
            if (errors.Count > 0)
                throw new Validation_Exception { ErrorMessages = errors };
        }

    }

}
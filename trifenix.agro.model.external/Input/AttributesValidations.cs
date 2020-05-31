using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.agro.db.exceptions;
using trifenix.agro.db.interfaces.agro.common;
using trifenix.agro.validator.interfaces;

namespace trifenix.agro.model.external.Input {
    
    public class UniqueValidation : IValidate {

        private readonly IExistElement ExistElement;
        public UniqueValidation(IExistElement existElement) {
            ExistElement = existElement;
        }

        public async Task Validate(object Obj, params object[] args) {
            var EntityType = (Type)args[0];
            var PropertyName = (string)args[1];
            var Id = (string)args[2]; ;
            if (Obj != null) {
                if (await AlreadyExists(Obj, EntityType, PropertyName, Id))
                    throw new Validation_Exception { ErrorMessages = new List<string> { $"{PropertyName} debe ser un atributo unico. Ya existe otro(a) {EntityType.Name} con valor '{(string)Obj}' en esta propiedad." } };
            }
        }

        private async Task<bool> AlreadyExists(object Obj, Type EntityType, string PropertyName, string Id){
            //TODO: Hacer conversor de InputBase a DocumentBase, deseable viceversa
            //string type = Obj.GetType().Name.ToLower();
            //switch (type) {
            //    case "string":
            //        return await(Task<bool>)ExistElement.GetType().GetMethod("ExistsWithPropertyValue").MakeGenericMethod(EntityType).Invoke(ExistElement, new object[] { PropertyName, Obj, Id });
            //    default:
            //        return false;
            //}
            return false;
        }

    }

    public class ReferenceValidation : IValidate {

        private readonly IExistElement ExistElement;
        public ReferenceValidation(IExistElement existElement) {
            ExistElement = existElement;
        }

        public async Task Validate(object reference, params object[] args) {
            var referencedEntityType = (Type)args[0];
            if (reference != null) {
                if (!(reference is string))
                    throw new Validation_Exception { ErrorMessages = new List<string> { "Referencia invalida. Debe ser de tipo string." } };
                if (string.IsNullOrWhiteSpace((string)reference) || string.IsNullOrEmpty((string)reference))
                    throw new Validation_Exception { ErrorMessages = new List<string> { "Referencia invalida. Debe contener un Id(string), no puede ser vacio ni espacio en blanco." } };
                if (!(await ExistEntityReferenced((string)reference, referencedEntityType)))
                    throw new Validation_Exception { ErrorMessages = new List<string> { $"La referencia es incorrecta. No existe un(a) {referencedEntityType.Name} con este id '{(string)reference}'." } };
            }
        }

        private async Task<bool> ExistEntityReferenced(string reference, Type referencedEntityType) =>
            await (Task<bool>)ExistElement.GetType().GetMethod("ExistsById").MakeGenericMethod(referencedEntityType).Invoke(ExistElement, new object[] { reference });

    }

    public class RequiredValidation : IValidate {

        public async Task Validate(object Obj, params object[] args) {
            if (Obj == null)
                throw new Validation_Exception { ErrorMessages = new List<string> { "Atributo requerido!" } };
            bool hasValue = true;
            if (Obj is IList) {
                if (Obj is Array)
                    hasValue = ((Array)Obj).Cast<dynamic>().Any();
                else
                    hasValue = ((IEnumerable<dynamic>)Obj).Any();
            }
            else if (Obj is string)
                hasValue = !string.IsNullOrWhiteSpace((string)Obj) && !string.IsNullOrEmpty((string)Obj);
            else if (Obj is DateTime)
                hasValue = !((DateTime)Obj).Equals(DateTime.MinValue);
            //else if (IsNumeric(Obj))
            //    hasValue = Convert.ToInt64(Obj) != 0;
            if(!hasValue)
                throw new Validation_Exception { ErrorMessages = new List<string> { "Atributo requerido!" } };
        }

        //private bool IsNumeric(object Obj) {
        //    switch (Type.GetTypeCode(Obj.GetType())) {
        //        case TypeCode.SByte:
        //        case TypeCode.Byte:
        //        case TypeCode.Int16:
        //        case TypeCode.UInt16:
        //        case TypeCode.Int32:
        //        case TypeCode.UInt32:
        //        case TypeCode.Int64:
        //        case TypeCode.UInt64:
        //        case TypeCode.Single:
        //        case TypeCode.Double:
        //        case TypeCode.Decimal:
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

    }

}
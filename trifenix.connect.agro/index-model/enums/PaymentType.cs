using System.ComponentModel;

namespace trifenix.connect.agro.index_model.enums
{
    public enum PaymentType
    {
        [Description("Débito")]
        DEBIT = 1,

        [Description("Crédito")]
        CREDIT = 2,

        [Description("Efectivo")]
        CASH = 3 
    }
}

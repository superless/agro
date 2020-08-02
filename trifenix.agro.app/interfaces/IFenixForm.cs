using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.db;
using trifenix.connect.agro_model_input;

namespace trifenix.agro.app.interfaces
{
    interface IFenixForm
    {
        void SetElements();

        string GetEntityName();

        string FriendlyName();

        bool Loading { get; set; }



        void Edit(object obj);



        void New();

        void ChangedList(object obj);

        object GetList();

        bool Valida();

        string Description();


    }
    
}

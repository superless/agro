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

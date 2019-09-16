namespace GoodsHandbookMalchikovPavlov.Model
{
    [Name("Home Appliances")]
    internal class HomeAppliances : Product
    {
        /// <summary>
        ///     Описание
        /// </summary>
        private string description;

        /// <summary>
        ///     Модель
        /// </summary>
        private string model;

        /// <summary>
        ///     Тип товара (телефон\холодильник)
        /// </summary>
        private string type;

        [Name("Home Appliance Type")]
        public string Type
        {
            get => type;
            set => type = value;
        }

        [Name("Home Appliance Model")]
        public string Model
        {
            get => model;
            set => model = value;
        }

        [Name("Home Appliance Description")]
        public string Description
        {
            get => description;
            set => description = value;
        }
    }
}
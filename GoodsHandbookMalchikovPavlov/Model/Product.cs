namespace GoodsHandbookMalchikovPavlov.Model
{
    internal class Product
    {
        /// <summary>
        ///     Компания производитель или издатель
        /// </summary>
        private string company;

        /// <summary>
        ///     Количество
        /// </summary>
        private int count;

        /// <summary>
        ///     Идентификатор
        /// </summary>
        private int id;

        /// <summary>
        ///     Название товара
        /// </summary>
        private string name;

        /// <summary>
        ///     Цена
        /// </summary>
        private float price;

        /// <summary>
        ///     Единица измерения (кг, шт, м)
        ///     ///
        /// </summary>
        private string unit;

        [AutoId]
        public int Id
        {
            get => id;
            set => id = value;
        }

        [Name("Product Name")]
        public string Name
        {
            get => name;
            set => name = value;
        }


        public int Count
        {
            get => count;
            set => count = value;
        }

        public float Price
        {
            get => price;
            set => price = value;
        }

        public string Unit
        {
            get => unit;
            set => unit = value;
        }

        public string Company
        {
            get => company;
            set => company = value;
        }
    }
}
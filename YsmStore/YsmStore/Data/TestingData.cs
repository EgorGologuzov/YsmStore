using System.Collections.Generic;

namespace YsmStore.Data
{
    public static class TestingData
    {
        public const string CUSTOMER_TEST_TOKEN = "customer_test_token";
        public const string ADMIN_TEST_TOKEN = "admin_test_token";
        public const string ADMIN_TEST_LOGIN = "admin";
        public const string ADMIN_TEST_PASSWORD = "123";
        public const string TEST_DESCRIPTION = "Apple iPhone 12 mini — супермощный смартфон в ультрамалом формате. Внутри компактного корпуса прописался целый набор передовых функций. Молниеносный процессор A14 Bionic восхищает невероятной производительностью. Изумительный дисплей Super Retina XDR радует сочной картинкой. Система камер удивляет качеством снимков, а зарядка MagSafe избавляет от хлопот с кабелем. Запись видео Dolby Vision — сильный козырь девайса.";
        public static readonly Dictionary<string, string> TEST_PROPERTIES = new Dictionary<string, string>()
        {
            { "Количество SIM-карт" , "2 (nano SIM+eSIM)" },
            { "Вес" , "135 г" },
            { "Размеры (ШxВxТ)" , "64.2x131.5x7.4 мм" },
            { "Оперативная память" , "4 ГБ" }
        };
    }
}

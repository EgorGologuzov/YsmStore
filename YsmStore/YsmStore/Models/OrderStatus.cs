using System.ComponentModel;

namespace YsmStore.Models
{
    public enum OrderStatus
    {
        [Description("Оформлен")] Processed,
        [Description("В доставке")] InDelivary,
        [Description("Ожидает получения")] AwaitPickUp,
        [Description("Получен")] Received,
        [Description("Отменен")] Cancaled
    }
}

using System.ComponentModel;

namespace NeUrokAdmin.Domain.Enums
{
    public enum ClientStatusEnum
    {
        [Description("Зарегистрирован")]
        Registered = 1,

        [Description("Интересуется услугами")]
        Interested = 2,

        [Description("Записан на курс")]
        Enrolled = 3,

        [Description("Обучается")]
        Learning = 4,

        [Description("Прошел обучение")]
        Completed = 5,

        [Description("В черном списке")]
        Blacklisted = 6
    }
}

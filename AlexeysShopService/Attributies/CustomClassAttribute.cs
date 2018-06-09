using System;

namespace AlexeysShopService.Attributies
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomClassAttribute : Attribute
    {
        public CustomClassAttribute(string descript)
        {
            Description = string.Format("Описание класса: ", descript);
        }

        public string Description { get; private set; }
    }
}
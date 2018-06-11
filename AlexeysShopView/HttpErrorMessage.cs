using System;
using System.Runtime.Serialization;
using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using AlexeysShopModel;

namespace AlexeysShopView
{
    [DataContract]
    public class HttpErrorMessage
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        [DataMember]
        public string MessageDetail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PET1.Domain.Models
{
    public class ResponseData<T>
    {
        public ResponseData() { }
        public ResponseData(bool success, T? data) => (Success, Data) = (success, data);
        public ResponseData(bool success, string message) => (Success, Message) = (success, message);
        // запрашиваемые данные
        public T? Data { get; set; }
        // признак успешного завершения запроса
        public bool Success { get; set; } = true;
        // сообщение в случае неуспешного завершения
        public string? Message { get; set; }
    }
}

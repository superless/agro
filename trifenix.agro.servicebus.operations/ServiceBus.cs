using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using trifenix.agro.model.external.Input;

namespace trifenix.agro.servicebus.operations {
    public class ServiceBus {

        private readonly MessageSender Sender;
        public ServiceBus(string connectionString, string queueName) {
            Sender = new MessageSender(connectionString, queueName);
        }

        // Convert an object to byte array
        public static byte[] Serialize(object obj) {
            string strSerial = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.ASCII.GetBytes(strSerial);
            return bytes;
        }

        // Convert a byte array to JObject(Json)
        public static JObject Deserialize(byte[] arrBytes) {
            string strSerial = Encoding.ASCII.GetString(arrBytes);
            var obj = (JObject)JsonConvert.DeserializeObject(strSerial);
            return obj;
        }

        private Message GetMessage(object obj, string sessionId) => new Message(Serialize(obj)) { SessionId = sessionId };

        public async Task PushElement(object obj, string sessionId = null) {
            var message = GetMessage(obj, sessionId);
            await Sender.SendAsync(message);
        }

    }

    public class OperationInstance<InputElement> where InputElement : InputBase {
        public InputElement Element;
        public Type EntityType;
        public string EntityName;
        public string HttpMethod;
        public string ObjectIdAAD;
        public OperationInstance(InputElement _element, string _entityName, string _httpMethod, string _objectIdAAD) {
            Element = _element;
            EntityType = _element.GetType();
            EntityName = _entityName;
            HttpMethod = _httpMethod;
            ObjectIdAAD = _objectIdAAD;
        }
    }

}
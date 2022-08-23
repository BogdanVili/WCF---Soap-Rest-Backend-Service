using Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    [ServiceKnownType(typeof(Firm))]
    [ServiceKnownType(typeof(Department))]
    [ServiceKnownType(typeof(Employee))]
    public interface IWorkerRequest
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddWorkerRest",
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        string AddWorkerRest(Firm firm, Department department, Employee employee);

        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateWorkerRest",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        string UpdateWorkerRest(Firm firm, Department department, Employee employee);

        [OperationContract]
        string DeleteWorkerRest(string message);

        [OperationContract]
        string AddWorkerSoap(string message);

        [OperationContract]
        string UpdateWorkerSoap(string message);

        [OperationContract]
        string DeleteWorkerSoap(string message);
    }
}

using Common.Model;
using Common.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WorkerService
{
    [ServiceContract]
    [ServiceKnownType(typeof(Firm))]
    [ServiceKnownType(typeof(Department))]
    [ServiceKnownType(typeof(Employee))]
    public interface IWorkerServiceRest
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
        [WebInvoke(UriTemplate = "/DeleteWorkerRest",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        string DeleteWorkerRest(DeleteParameters deleteParameters);
    }
}

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
    public interface IWorkerServiceSoap
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddWorkerSoap",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml)]
        string AddWorkerSoap(Firm firm, Department department, Employee employee);

        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateWorkerSoap",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml)]
        string UpdateWorkerSoap(Firm firm, Department department, Employee employee);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteWorkerSoap",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml)]
        string DeleteWorkerSoap(DeleteParameters deleteParameters);
    }
}

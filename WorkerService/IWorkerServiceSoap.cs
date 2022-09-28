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
    [ServiceKnownType(typeof(FirmRequest))]
    [ServiceKnownType(typeof(DepartmentRequest))]
    [ServiceKnownType(typeof(EmployeeRequest))]
    public interface IWorkerServiceSoap
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddWorkerSoap",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml)]
        string AddWorkerSoap(FirmRequest firmRequest, DepartmentRequest departmentRequest, EmployeeRequest employeeRequest);

        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateWorkerSoap",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml)]
        string UpdateWorkerSoap(FirmRequest firmRequest, DepartmentRequest departmentRequest, EmployeeRequest employeeRequest);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteWorkerSoap",
           Method = "POST",
           BodyStyle = WebMessageBodyStyle.Wrapped,
           RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml)]
        string DeleteWorkerSoap(DeleteParameters deleteParameters);
    }
}

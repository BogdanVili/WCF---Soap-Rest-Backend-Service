﻿using Common.Model;
using Common.ModelRequest;
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
        #region REST
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
        #endregion

        #region SOAP
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
        #endregion
    }
}

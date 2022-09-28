using Common.Model;
using Common.ModelCSV;
using System.Collections.Generic;
using System.Linq;

namespace WorkerServer
{
    internal class QueryConstructor
    {
        Validation validation;
        ModelGetter modelGetter;

        public QueryConstructor(Validation validation, ModelGetter modelGetter)
        {
            this.validation = validation;
            this.modelGetter = modelGetter;
        }

        public string Add(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            if (!validation.CheckIfFirmExists(firm.Name))
            {
                returnQuery += SqlQueryBuilder.InsertFirmBuilder(firm);
            }

            if (!validation.CheckIfDepartmentExists(department.Id))
            {
                returnQuery += SqlQueryBuilder.InsertDepartmentBuilder(department);
            }

            if (!validation.CheckIfEmployeeExists(employee.JMBG))
            {
                returnQuery += SqlQueryBuilder.InsertEmployeeBuilder(employee);
            }

            return returnQuery;
        }

        public string Delete(string firmName, int departmentId, long employeeId)
        {
            string returnQuery = "";

            if (firmName != null)
            {
                DeleteFirm(firmName, ref returnQuery);
            }

            if (departmentId > 0)
            {
                DeleteDepartment(departmentId, ref returnQuery);
            }

            if (employeeId > 0)
            {
                DeleteEmployee(employeeId, ref returnQuery);
            }

            return returnQuery;
        }

        private void DeleteDepartment(int departmentId, ref string returnQuery)
        {
            Department _department = modelGetter.GetDepartmentById(departmentId);

            List<Working> _workings = modelGetter.GetWorkingsByDepartmentId(_department.Id);

            foreach (Working _working in _workings)
            {
                returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
            }

            foreach (int _departmentId in _workings.Select(w => w.DepartmentId).Distinct())
            {
                returnQuery += SqlQueryBuilder.DeleteDepartmentBuilder(_departmentId);
            }

            foreach (int _employeeId in _workings.Select(w => w.EmployeeId).Distinct())
            {
                returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employeeId);
            }
        }

        private void DeleteEmployee(long employeeId, ref string returnQuery)
        {
            Employee _employee = modelGetter.GetEmployeeById(employeeId);
            List<Working> _workings = modelGetter.GetWorkingsByEmployeeId(_employee.JMBG);

            foreach (Working _working in _workings)
            {
                returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
            }

            foreach (int _employeeId in _workings.Select(w => w.EmployeeId).Distinct())
            {
                returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employeeId);
            }
        }

        private void DeleteFirm(string firmName, ref string returnQuery)
        {
            Firm _firm = modelGetter.GetFirmByName(firmName);

            List<Working> _workings = modelGetter.GetWorkingsByFirmId(_firm.Id);

            foreach (Working _working in _workings)
            {
                returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
            }

            foreach (int _firmId in _workings.Select(w => w.FirmId).Distinct())
            {
                returnQuery += SqlQueryBuilder.DeleteFirmBuilder(_firmId);
            }

            foreach (int _departmentId in _workings.Select(w => w.DepartmentId).Distinct())
            {
                returnQuery += SqlQueryBuilder.DeleteDepartmentBuilder(_departmentId);
            }

            foreach (int _employeeId in _workings.Select(w => w.EmployeeId).Distinct())
            {
                returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employeeId);
            }
        }

        public string Update(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            //returnQuery += SqlQueryBuilder.UpdateFirmBuilder(firm);

            returnQuery += SqlQueryBuilder.UpdateDepartmentBuilder(department);

            returnQuery += SqlQueryBuilder.UpdateEmployeeBuilder(employee);

            return returnQuery;
        }

        public string UpdateEmployeeData(EmployeeUpdateData employeeUpdateData)
        {
            string returnQuery = "";

            if (validation.CheckIfEmployeeExists(employeeUpdateData.JMBG))
            {
                returnQuery += SqlQueryBuilder.UpdateEmployeeDataBuilder(employeeUpdateData);
            }

            return returnQuery;
        }
    }
}
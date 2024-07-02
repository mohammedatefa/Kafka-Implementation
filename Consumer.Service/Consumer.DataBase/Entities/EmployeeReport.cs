namespace Consumer.DataBase.Entities
{
    public class EmployeeReport
    {
        public EmployeeReport(Guid id, Guid employeeId, string name, string departmentName)
        {
            Id = id;
            Name = name;
            EmployeeId = employeeId;
            DepartmentName = departmentName;
        }

        public Guid Id { get; init; }
        public Guid EmployeeId { get; init; }
        public string Name { get; init; }
        public string DepartmentName { get; init; }
    }
}

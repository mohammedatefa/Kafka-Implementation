namespace Consumer.Shared
{
    public record Employee
    {
        public Employee(Guid id, string name, string departmentName)
        {
            Id = id;
            Name = name;
            DepartmentName = departmentName;
        }

        public Guid Id { get; init; }
        public string Name { get; init; }
        public string DepartmentName { get; init; }
    }
}

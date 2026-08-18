using System;
using System.Collections.Generic;

namespace SOLD.SingleResponsibility
{
    /// <summary>
    /// SOLD - Single Responsibility Principle (S)
    /// Extended implementation showing real-world scenarios
    /// Each class should have only one reason to change
    /// </summary>

    // ========== VIOLATION: God Class - Too Many Responsibilities ==========
    public class BadEmployeeManager
    {
        private List<string> employees = new List<string>();

        public void AddEmployee(string name)
        {
            employees.Add(name);
            Console.WriteLine($"Employee {name} added to database");
        }

        public void RemoveEmployee(string name)
        {
            employees.Remove(name);
            Console.WriteLine($"Employee {name} removed from database");
        }

        public void CalculateSalary(string name, double baseSalary)
        {
            double tax = baseSalary * 0.15;
            double netSalary = baseSalary - tax;
            Console.WriteLine($"{name}'s salary: ${netSalary}");
        }

        public void GeneratePayroll()
        {
            Console.WriteLine("Generating payroll report...");
            Console.WriteLine("Payroll generated and sent to accounting");
        }

        public void SendSalaryNotification(string name, string email)
        {
            Console.WriteLine($"Sending salary notification to {email}");
        }

        public void ApplyLeave(string name, int days)
        {
            Console.WriteLine($"{name} applied for {days} days leave");
        }

        public void RecordAttendance(string name, string status)
        {
            Console.WriteLine($"{name} marked as {status}");
        }
    }

    // ========== SOLUTION: Separate Each Responsibility ==========

    // 1. Employee Data Management
    public class EmployeeRepository
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
            Console.WriteLine($"Employee {employee.Name} added to database");
        }

        public void RemoveEmployee(string name)
        {
            _employees.RemoveAll(e => e.Name == name);
            Console.WriteLine($"Employee {name} removed from database");
        }

        public Employee GetEmployee(string name)
        {
            return _employees.Find(e => e.Name == name);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employees;
        }
    }

    // 2. Employee Model
    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public double BaseSalary { get; set; }
        public int LeaveDays { get; set; }
    }

    // 3. Salary Calculation
    public class SalaryCalculator
    {
        private const double TaxRate = 0.15;
        private const double HealthInsuranceDeduction = 500;

        public double CalculateNetSalary(double baseSalary)
        {
            double tax = baseSalary * TaxRate;
            double deductions = tax + HealthInsuranceDeduction;
            return baseSalary - deductions;
        }

        public SalaryDetails GetSalaryDetails(Employee employee)
        {
            double tax = employee.BaseSalary * TaxRate;
            double netSalary = CalculateNetSalary(employee.BaseSalary);

            return new SalaryDetails
            {
                EmployeeName = employee.Name,
                GrossSalary = employee.BaseSalary,
                Tax = tax,
                Deductions = HealthInsuranceDeduction,
                NetSalary = netSalary
            };
        }
    }

    public class SalaryDetails
    {
        public string EmployeeName { get; set; }
        public double GrossSalary { get; set; }
        public double Tax { get; set; }
        public double Deductions { get; set; }
        public double NetSalary { get; set; }

        public override string ToString()
        {
            return $"Employee: {EmployeeName}\nGross: ${GrossSalary}\nTax: ${Tax}\nDeductions: ${Deductions}\nNet: ${NetSalary}";
        }
    }

    // 4. Attendance Management
    public class AttendanceManager
    {
        private Dictionary<string, List<string>> _attendance = new Dictionary<string, List<string>>();

        public void RecordAttendance(string employeeName, string status)
        {
            if (!_attendance.ContainsKey(employeeName))
            {
                _attendance[employeeName] = new List<string>();
            }
            _attendance[employeeName].Add($"{DateTime.Now:yyyy-MM-dd} - {status}");
            Console.WriteLine($"{employeeName} marked as {status}");
        }

        public List<string> GetAttendance(string employeeName)
        {
            return _attendance.ContainsKey(employeeName) ? _attendance[employeeName] : new List<string>();
        }
    }

    // 5. Leave Management
    public class LeaveManager
    {
        private Dictionary<string, int> _leaveBalance = new Dictionary<string, int>();

        public void ApplyLeave(string employeeName, int days)
        {
            if (!_leaveBalance.ContainsKey(employeeName))
            {
                _leaveBalance[employeeName] = 20; // Default 20 days
            }

            if (_leaveBalance[employeeName] >= days)
            {
                _leaveBalance[employeeName] -= days;
                Console.WriteLine($"{employeeName} leave approved for {days} days. Remaining: {_leaveBalance[employeeName]}");
            }
            else
            {
                Console.WriteLine($"{employeeName} insufficient leave balance");
            }
        }

        public int GetLeaveBalance(string employeeName)
        {
            return _leaveBalance.ContainsKey(employeeName) ? _leaveBalance[employeeName] : 0;
        }
    }

    // 6. Payroll Report Generation
    public class PayrollReportGenerator
    {
        public void GeneratePayroll(List<SalaryDetails> salaryDetails)
        {
            Console.WriteLine("\n========== PAYROLL REPORT ==========");
            double totalGross = 0;
            double totalDeductions = 0;

            foreach (var salary in salaryDetails)
            {
                Console.WriteLine(salary);
                Console.WriteLine();
                totalGross += salary.GrossSalary;
                totalDeductions += salary.Tax + salary.Deductions;
            }

            Console.WriteLine($"Total Gross Salary: ${totalGross}");
            Console.WriteLine($"Total Deductions: ${totalDeductions}");
            Console.WriteLine($"Net Payroll: ${totalGross - totalDeductions}");
            Console.WriteLine("====================================\n");
        }
    }

    // 7. Notification Service
    public class NotificationService
    {
        public void SendSalaryNotification(Employee employee, SalaryDetails details)
        {
            Console.WriteLine($"Sending salary slip to {employee.Email}");
            Console.WriteLine($"Subject: Salary Slip for {DateTime.Now:MMMM yyyy}");
            Console.WriteLine($"Body: Your salary has been processed. Net amount: ${details.NetSalary}");
        }

        public void SendLeaveNotification(string employeeName, string email, string message)
        {
            Console.WriteLine($"Sending notification to {email}: {message}");
        }
    }

    // ========== USAGE EXAMPLE ==========
    public class SRPDemoSOLD
    {
        public static void Main()
        {
            Console.WriteLine("===== SOLD: Single Responsibility Principle (S) =====\n");

            // Initialize services
            var employeeRepo = new EmployeeRepository();
            var salaryCalc = new SalaryCalculator();
            var attendance = new AttendanceManager();
            var leave = new LeaveManager();
            var payrollGen = new PayrollReportGenerator();
            var notificationService = new NotificationService();

            // Create employees
            var emp1 = new Employee { Name = "John Doe", Email = "john@example.com", BaseSalary = 5000, LeaveDays = 20 };
            var emp2 = new Employee { Name = "Jane Smith", Email = "jane@example.com", BaseSalary = 6000, LeaveDays = 20 };

            // Add employees
            employeeRepo.AddEmployee(emp1);
            employeeRepo.AddEmployee(emp2);

            Console.WriteLine("\n--- Recording Attendance ---");
            attendance.RecordAttendance("John Doe", "Present");
            attendance.RecordAttendance("Jane Smith", "Present");

            Console.WriteLine("\n--- Applying Leave ---");
            leave.ApplyLeave("John Doe", 2);
            leave.ApplyLeave("Jane Smith", 3);

            Console.WriteLine("\n--- Processing Payroll ---");
            var salaryDetails = new List<SalaryDetails>();
            foreach (var emp in employeeRepo.GetAllEmployees())
            {
                var details = salaryCalc.GetSalaryDetails(emp);
                salaryDetails.Add(details);
            }

            payrollGen.GeneratePayroll(salaryDetails);

            Console.WriteLine("--- Sending Notifications ---");
            foreach (var emp in employeeRepo.GetAllEmployees())
            {
                var details = salaryCalc.GetSalaryDetails(emp);
                notificationService.SendSalaryNotification(emp, details);
            }

            Console.WriteLine("\n✓ Each class has a single, well-defined responsibility");
            Console.WriteLine("✓ Changes to salary calculation don't affect attendance tracking");
            Console.WriteLine("✓ Changes to notifications don't affect leave management");
        }
    }
}

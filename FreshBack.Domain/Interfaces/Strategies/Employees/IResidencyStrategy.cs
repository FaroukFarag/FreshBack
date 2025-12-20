using FreshBack.Domain.Enums.Employees;

namespace FreshBack.Domain.Interfaces.Strategies.Employees;

public interface IResidencyStrategy
{
    string GetResidencyStatus();
    bool IsValid(int employeeId);
    bool CanHandle(ResidencyStatus residencyStatus);
}

using Ardalis.SmartEnum;
using JobManagement.Domain.Common;

namespace JobManagement.Domain.Users.ValueObjects;

public class UserRole : SmartEnum<UserRole>, IValueObject
{

    public static readonly UserRole Operator = new UserRole(nameof(Operator), 1);
    public static readonly UserRole Reader = new UserRole(nameof(Reader), 2);

    private UserRole(string name, int value) : 
        base(name, value) { }

}

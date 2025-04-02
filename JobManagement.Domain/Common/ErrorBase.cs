using FluentResults;

namespace JobManagement.Domain.Common;

public class ErrorBase : Error
{

    public int ErrorCode { get; }
    public int GroupCode { get; }

    public ErrorBase(int groupCode, int code, string message) :
        base(message)
    {
        GroupCode = groupCode;
        ErrorCode = code;
    }

}

using Microsoft.AspNetCore.SignalR;

namespace JobManagement.API.Hubs;

public class JobManagerHub : Hub
{

    public const string JobStatusChangeMethodName = "JobStatusChanged";

}

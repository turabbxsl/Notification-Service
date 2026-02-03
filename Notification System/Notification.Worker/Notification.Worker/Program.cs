using Notification.Core.Models;
using Notification.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<NotificationQueue>();
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();

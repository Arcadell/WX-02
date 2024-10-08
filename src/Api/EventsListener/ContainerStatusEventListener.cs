﻿using Api.Hubs;
using Docker;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Api.EventsListener
{
    public static class ContainerStatusEventListener
    {
        public static IServiceCollection AddContainerStatusEventListener(this IServiceCollection services)
        {
            services.AddDockerContainerMonitor(opt =>
            {
                opt.AddMessageHandler((sp, message) =>
                {
                    var hub = sp.GetRequiredService<IHubContext<ContainerHub>>();
                    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("Container monitor");
                    
                    logger.LogTrace("Event: {action} for container {id}", message.Action, message.ID);

                    return message.Action switch
                    {
                        "start" => hub.Clients.All.SendAsync("NewContainerStatus", message.ID,
                            ContainerStatusEvent.STARTED),
                        "stop" => hub.Clients.All.SendAsync("NewContainerStatus", message.ID,
                            ContainerStatusEvent.DIED),
                        _ => Task.CompletedTask
                    };
                });
            });

            return services;
        }
    }
}
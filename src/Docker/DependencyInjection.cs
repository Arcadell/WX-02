﻿using Docker.Commands;
using Docker.Commands.Interfaces;
using Docker.DotNet;
using Docker.Models;
using Docker.Monitors;
using Docker.Monitors.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Docker;

public static class DependencyInjection
{
    public static IServiceCollection AddDockerServices(this IServiceCollection services)
    {
        services.AddSingleton<IDockerClient>(_ => new DockerClientConfiguration().CreateClient());
        

        services.AddScoped<IContainerCommands, ContainerCommands>();
        services.AddScoped<IImageCommands, ImageCommands>();
        services.AddScoped<IVolumeCommands, VolumeCommands>();

        services.AddHostedService<ContainerMonitorBackgroundService>();
        return services;
    }

    public static IServiceCollection AddDockerContainerMonitor(this IServiceCollection services, Action<DockerContainerMonitorOptions>configureOptions)
    {
        services.AddDockerServices();
        services.Configure(configureOptions);
        services.AddSingleton<IContainerMonitorService, ContainerMonitorService>();
        return services;
    }
}
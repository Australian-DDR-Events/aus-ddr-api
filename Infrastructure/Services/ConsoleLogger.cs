
using System;
using System.Threading.Tasks;
using Application.Core.Interfaces;

namespace Infrastructure.Services;

public class ConsoleLogger : ILogger
{
    public Task Info(string data)
    {
        Console.WriteLine($"[INFO] {DateTime.Now.ToUniversalTime():s} - ${data}");
        return Task.CompletedTask;
    }

    public Task Warn(string data)
    {
        Console.WriteLine($"[WARN] {DateTime.Now.ToUniversalTime():s} - ${data}");
        return Task.CompletedTask;
    }

    public Task Error(string data)
    {
        Console.WriteLine($"[ERROR] {DateTime.Now.ToUniversalTime():s} - ${data}");
        return Task.CompletedTask;
    }
}
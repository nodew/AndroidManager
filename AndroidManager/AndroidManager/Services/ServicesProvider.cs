using Microsoft.Extensions.DependencyInjection;
using System;

namespace AndroidManager.Services 
{
    public static class ServicesProvider {
        public static IServiceProvider Current => App.Current.Services;

        public static T GetService<T>() => Current.GetService<T>();
    }
}
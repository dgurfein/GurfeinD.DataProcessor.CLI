using Application.Registrations;
using DataProcessor.Application.DTOs;
using DataProcessor.Application.Interfaces;
using DataProcessor.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor.Application.Registration
{
    public static class ApplicationServiceInitializer
    {
        // Registers all services for the MissingNumber use case
        public static void ServicesRegistration(this IServiceCollection services)
        {
            // use cases
            // Missing number
            services.AddSingleton<IParser<MissingNumberRequest>, MissingNumberParser>(); 
            services.AddSingleton<IValidator<MissingNumberRequest>, MissingNumberValidator>();
            services.AddSingleton<IProcessor<MissingNumberRequest,MissingNumberResult>, MissingNumberProcessor>();
            services.AddSingleton<IUseCase, MissingNumberUseCase>();

            // register the resolver
            services.AddSingleton<IUseCaseResolver, UseCaseResolver>();
        }
    }
}

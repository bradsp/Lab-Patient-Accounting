﻿using System;
using System.Threading;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using Quartz;

namespace Lab_Billing_Jobs
{
    public partial class JobProcessor
    {
        public class SimpleJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"SimpleJob running at {DateTime.Now}");


                return Task.CompletedTask;
            }
        }
    }
}
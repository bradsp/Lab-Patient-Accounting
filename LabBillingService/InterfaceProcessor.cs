﻿using System.Timers;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;


namespace LabBillingService
{
    public class InterfaceProcessor
    {
        private readonly System.Timers.Timer _timer;

        public InterfaceProcessor()
        {
            _timer = new System.Timers.Timer(15000) { AutoReset = true };
            _timer.Elapsed += Process;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Paused()
        {
            _timer.Stop();
        }

        public void Continue()
        {
            _timer.Start();
        }

        public void Process(object sender, ElapsedEventArgs args)
        {
            _timer.Stop();
            System.Console.WriteLine("Processing messages.");

            HL7ProcessorService hl7Processor = new (Program.AppEnvironment);
            hl7Processor.ProcessMessages();

            System.Console.WriteLine("Messages processed. Waiting on new messages.");
            _timer.Start();
        }


    }
}

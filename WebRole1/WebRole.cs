using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Quartz;
using Quartz.Impl;
using System.Diagnostics;

/**
 * Derived from http://blogs.msdn.com/b/chgeuer/archive/2010/08/30/running-perl-scripts-in-windows-azure-is-trivial.aspx
 * Differences:
 *  Perl installed in startup task (retrieved from blob storage)
 *  Perl modules also installed
 *  This is a web role, instead of a worker role
 *  The script is run on a schedule, using Quartz.net
 */
namespace WebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            // Configure diagnostics
            // From: http://www.packtpub.com/article/digging-into-windows-azure-diagnostics-tutotial,
            // need to add a diagnostics trace listener; (web.config is just for website tracing)
            Trace.Listeners.Add(new DiagnosticMonitorTraceListener());
            Trace.AutoFlush = true;
            Trace.TraceInformation("Information");

            // Set up to transfer to blob storage
            DiagnosticMonitorConfiguration diagnosticConfiguration = DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagnosticConfiguration.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d); // transfer every 1 mins, just for testing
            diagnosticConfiguration.Logs.BufferQuotaInMB = 10;
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticConfiguration);

            // Create a quartz scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            // Create a job and a trigger - trigger the job to run every 1 minute
            IJobDetail job = JobBuilder.Create<PerlJob>()
                                    .WithIdentity("perljob")
                                    .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("perltrigger")
                .StartNow()
                .WithCronSchedule("0 0/1 * * * ?") // every 1 minute
                .Build();
            var nextRunTime = scheduler.ScheduleJob(job, trigger);
            Trace.TraceInformation("perljob scheduled for next run at" + nextRunTime.ToLocalTime());

            return base.OnStart();
        }
    }
}

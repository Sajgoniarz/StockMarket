using Quartz;
using Quartz.Impl;
using StockMarket.Workers.Jobs;

namespace StockMarket.Workers
{
    public class Scheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<StocksDownloaderJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(s => s
                    .WithIntervalInSeconds(20)
                    .RepeatForever()
                ).Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
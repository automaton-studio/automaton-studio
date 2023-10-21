using Automaton.Studio.Server.Enums;
using Automaton.Studio.Server.Models;
using Hangfire;
using Hangfire.MemoryStorage;

namespace Automaton.Studio.Server.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var jobStorage = new MemoryStorage();
            GlobalConfiguration.Configuration.UseStorage(jobStorage);
        }

        [Test]
        [Description("Minutely cron generation")]
        public void MinutelyCronTest()
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Minutely)
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Minutely();

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));         
        }

        [Test]
        [Description("Hourly default cron generation")]
        public void HourlyDefaultCronTest()
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Hourly)
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Hourly();

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(0)]
        [TestCase(59)]
        [Description("Hourly with valid minutes cron generation")]
        public void HourlyWithMinutesCronTest(int minute)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Hourly)
                {
                    Minute = minute
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Hourly(minute);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [Description("Daily default cron generation")]
        public void DailyDefaultCronTest()
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Daily)
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Daily();

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(0)]
        [TestCase(23)]
        [Description("Daily with hours cron generation")]
        public void DailyWithValidHoursCronTest(int hour)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Daily)
                {
                    Hour = hour
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Daily(hour);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(23, 59)]
        [Description("Daily with hours and minutes cron generation")]
        public void DailyWithHoursAndMinutesCronTest(int hour, int minute)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Daily)
                {
                    Hour = hour,
                    Minute = minute
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Daily(hour, minute);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [Description("Monthly default cron generation")]
        public void MonthlyDefaultCronTest()
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Monthly)
                {
                    Day = 1
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Monthly();

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(1)]
        [TestCase(30)]
        [Description("Monthly with day cron generation")]
        public void MonthlyWithDaysCronTest(int day)
        {
            var schedule = new ScheduleModel
            {
                Id = Guid.NewGuid(),
                CronRecurrence = new CronRecurrence(CronType.Monthly)
                {
                    Day = day
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Monthly(day);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(30, 23)]
        [Description("Monthly with day and hour cron generation")]
        public void MonthlyWithDaysAndHoursCronTest(int day, int hour)
        {
            var schedule = new ScheduleModel
            {
                Id = Guid.NewGuid(),
                CronRecurrence = new CronRecurrence(CronType.Monthly)
                {
                    Day = day,
                    Hour = hour
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Monthly(day, hour);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(1, 0, 0)]
        [TestCase(30, 23, 59)]
        [Description("Monthly with days, hours and minutes cron generation")]
        public void MonthlyWithDaysHoursAndMinutesCronTest(int day, int hour, int minute)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Monthly)
                {
                    Day = day,
                    Hour = hour,
                    Minute = minute
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Monthly(day, hour, minute);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        public void TestMethod()
        {
            Console.WriteLine("Test method");
        }
    }
}
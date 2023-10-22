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
            GlobalConfiguration.Configuration.UseStorage(new MemoryStorage());
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
        [Description("Weekly default cron generation")]
        public void WeeklyDefaultCronTest()
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Weekly)
                {
                    DayOfWeek = DayOfWeek.Monday
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Weekly();

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Saturday)]
        [Description("Weekly with day of the week cron generation")]
        public void WeeklyWithDayCronTest(DayOfWeek dayOfWeek)
        {
            var schedule = new ScheduleModel
            {
                Id = Guid.NewGuid(),
                CronRecurrence = new CronRecurrence(CronType.Weekly)
                {
                    DayOfWeek = dayOfWeek
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Weekly(dayOfWeek);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(DayOfWeek.Sunday, 0)]
        [TestCase(DayOfWeek.Saturday, 23)]
        [Description("Weekly with day of the week and hour cron generation")]
        public void WeeklyWithDayAndHourCronTest(DayOfWeek dayOfWeek, int hour)
        {
            var schedule = new ScheduleModel
            {
                Id = Guid.NewGuid(),
                CronRecurrence = new CronRecurrence(CronType.Weekly)
                {
                    DayOfWeek = dayOfWeek,
                    Hour = hour
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Weekly(dayOfWeek, hour);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(DayOfWeek.Sunday, 0, 0)]
        [TestCase(DayOfWeek.Saturday, 23, 59)]
        [Description("Weekly with day of the week hour and minute cron generation")]
        public void WeeklyWithDayHourAndMinuteCronTest(DayOfWeek dayOfWeek, int hour, int minute)
        {
            var schedule = new ScheduleModel
            {
                Id = Guid.NewGuid(),
                CronRecurrence = new CronRecurrence(CronType.Weekly)
                {
                    DayOfWeek = dayOfWeek,
                    Hour = hour,
                    Minute = minute
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Weekly(dayOfWeek, hour, minute);

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
        [TestCase(31)]
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
        [TestCase(31, 23)]
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
        [TestCase(31, 23, 59)]
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

        [Test]
        [TestCase(1)]
        [TestCase(12)]
        [Description("Yearly with month cron generation")]
        public void YearlyWithMonthCronTest(int month)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Yearly)
                {
                    Month = month,
                    Day = 1
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Yearly(month);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(12, 30)]
        [Description("Yearly with month and days cron generation")]
        public void YearlyWithMonthDaysCronTest(int month, int day)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Yearly)
                {
                    Month = month,
                    Day = day
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Yearly(month, day);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(1, 1, 0)]
        [TestCase(12, 30, 23)]
        [Description("Yearly with month, days, hours cron generation")]
        public void YearlyWithMonthDaysHoursCronTest(int month, int day, int hour)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Yearly)
                {
                    Month = month,
                    Day = day,
                    Hour = hour
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Yearly(month, day, hour);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        [Test]
        [TestCase(1, 1, 0, 0)]
        [TestCase(12, 30, 23, 59)]
        [Description("Yearly with month, days, hours and minutes cron generation")]
        public void YearlyWithMonthDaysHoursAndMinutesCronTest(int month, int day, int hour, int minute)
        {
            var schedule = new ScheduleModel
            {
                CronRecurrence = new CronRecurrence(CronType.Yearly)
                {
                    Month = month,
                    Day = day,
                    Hour = hour,
                    Minute = minute
                }
            };

            var actualCron = schedule.GetCron();
            var expectedCron = Cron.Yearly(month, day, hour, minute);

            Assert.That(actualCron, Is.EqualTo(expectedCron));
            Assert.DoesNotThrow(() => RecurringJob.AddOrUpdate(schedule.Id.ToString(), () => TestMethod(), schedule.GetCron()));
        }

        public void TestMethod()
        {
            Console.WriteLine("Test method");
        }
    }
}
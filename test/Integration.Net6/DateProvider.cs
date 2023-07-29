using System;

namespace Integration.Net6;

public class DateProvider : IDateProvider
{
    public DateTimeOffset GetDate() => DateTimeOffset.UtcNow;
}

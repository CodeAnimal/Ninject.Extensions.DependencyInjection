using System;

namespace Integration.Net6;

public interface IDateProvider
{
    DateTimeOffset GetDate();
}

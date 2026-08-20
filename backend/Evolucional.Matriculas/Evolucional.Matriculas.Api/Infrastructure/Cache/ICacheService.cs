using System;

namespace Evolucional.Matriculas.Api.Infrastructure.Cache
{
    public interface ICacheService
    {
        T Get<T>(string key);

        void Set<T>(
            string key,
            T value,
            TimeSpan expiration);

        void Remove(string key);
    }
}
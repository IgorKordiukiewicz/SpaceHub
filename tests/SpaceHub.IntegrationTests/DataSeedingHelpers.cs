using Bogus;

namespace SpaceHub.IntegrationTests;

public static class DataSeedingHelpers
{
    public static string JoinedWords(this Faker faker, int wordsCount = 3)
    {
        return string.Join(" ", faker.Lorem.Words(wordsCount));
    }
}

namespace Domain.Contracts
{
    public interface IDataSeed
    {
        public Task SeedDataAsync();
        public Task SeedIdentityDataAsync();
    }
}

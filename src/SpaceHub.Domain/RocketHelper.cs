namespace SpaceHub.Domain;

public static class RocketHelper
{
    public static int? GetCostPerKgToOrbit(long? launchCost, int? orbitCapacity)
        => (launchCost is not null && orbitCapacity is not null && orbitCapacity > 0) ? (int)(launchCost.Value / orbitCapacity.Value) : null;

    public static int GetLaunchSuccessPercent(int successfulLaunches, int totalLaunches)
        => totalLaunches > 0 ? (int) Math.Round((double)successfulLaunches * 100 / totalLaunches) : 0;
}

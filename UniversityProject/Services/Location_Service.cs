﻿using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class LocationService : ILocationService
{
    private readonly LocationSQL _locationDatabase;

    public LocationService(string dbPath)
    {
        _locationDatabase = new LocationSQL(dbPath);
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await Task.Run(() => _locationDatabase.GetAllLocations());
    }

    public async Task<Location> GetLocationByIdAsync(string locationId)
    {
        return await Task.Run(() => _locationDatabase.GetLocationById(locationId));
    }

    public async Task InsertLocationAsync(Location location)
    {
        await Task.Run(() => _locationDatabase.InsertLocation(location));
    }

    public async Task UpdateLocationAsync(Location location)
    {
        await Task.Run(() => _locationDatabase.UpdateLocation(location));
    }

    public async Task DeleteLocationAsync(string locationId)
    {
        await Task.Run(() => _locationDatabase.DeleteLocation(locationId));
    }
    public async Task<Location> GetLocationIdAsync(string locationid)
    {
        return await Task.Run(() => _locationDatabase.GetLocationById(locationid));
    }
}

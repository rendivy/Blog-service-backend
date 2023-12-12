using blog_backend.DAO.Database;
using blog_backend.DAO.IService;
using blog_backend.DAO.Model;
using blog_backend.Entity;
using blog_backend.Migrations;
using blog_backend.Service.Account.Extensions;
using blog_backend.Service.Extensions;
using Microsoft.EntityFrameworkCore;

namespace blog_backend.Service;

public class GarService : IGarService
{
    private readonly GarDbContext _garContext;

    public GarService(GarDbContext garContext)
    {
        _garContext = garContext;
    }

    private Task<List<AddressDTO>> GetObjectAsync(int parentObjectId, string query)
    {
        var lowerQuery = query.ToLower();
        return _garContext.AsAdmHierarchies
            .Join(_garContext.AsAddrObjs,
                asAdmHierarchy => asAdmHierarchy.Objectid,
                asAddrObj => asAddrObj.Objectid,
                (asAdmHierarchy, asAddrObj) => new { asAdmHierarchy, asAddrObj })
            .Where(joined =>
                joined.asAdmHierarchy.Parentobjid == parentObjectId && joined.asAddrObj.Isactive == 1 &&
                joined.asAddrObj.Isactual == 1 && joined.asAddrObj.Normalize_name.Contains(lowerQuery))
            .Select(joined => new AddressDTO
            {
                ObjectId = joined.asAddrObj.Objectid,
                ObjectGuid = joined.asAddrObj.Objectguid,
                Text = $"{joined.asAddrObj.Typename} {joined.asAddrObj.Name}",
                Level = GarExtensions.ToGarAddressLevel(joined.asAddrObj.Level).ToString(),
                ObjectLevelText =
                    GarExtensions.ObjectLevelToString(GarExtensions.ToGarAddressLevel(joined.asAddrObj.Level))
            })
            .Take(20)
            .ToListAsync();
    }

    private Task<List<AddressDTO>> GetHousesAsync(int parentObjectId, string query)
    {
        return _garContext.AsAdmHierarchies
            .Join(_garContext.AsHouses,
                asAdmHierarchy => asAdmHierarchy.Objectid,
                asHouse => asHouse.Objectid,
                (asAdmHierarchy, asHouse) => new { asAdmHierarchy, asHouse })
            .Where(joined =>
                joined.asAdmHierarchy.Parentobjid == parentObjectId &&
                joined.asHouse.Isactive == 1 && joined.asHouse.Isactual == 1 &&
                joined.asHouse.HouseFullNum.Contains(query))
            .Select(joined => new AddressDTO
            {
                ObjectId = joined.asHouse.Objectid,
                ObjectGuid = joined.asHouse.Objectguid,
                Text = joined.asHouse.HouseFullNum,
                Level = GarExtensions.ToGarAddressLevel("10").ToString(),
                ObjectLevelText = GarExtensions.ObjectLevelToString(GarExtensions.ToGarAddressLevel("10"))
            }).Take(10).ToListAsync();
    }


    public async Task<List<AddressDTO>> SearchAddressAsync(int parentObjectId, string query)
    {
        var addressObjects = await GetObjectAsync(parentObjectId, query);
        var houses = await GetHousesAsync(parentObjectId, query);
        var result = addressObjects.Concat(houses).ToList();
        return result;
    }

    public async Task<List<AddressDTO>> GetAllHierarchy(Guid objectGuid)
    {
        var houseObject = await _garContext.AsHouses
            .FirstOrDefaultAsync(house => house.Objectguid == objectGuid);

        var addressObject = await _garContext.AsAddrObjs
            .FirstOrDefaultAsync(address => address.Objectguid == objectGuid);

        string? path;

        if (houseObject == null)
        {
            path = await _garContext.AsAdmHierarchies.Where(obj => obj.Objectid == addressObject.Objectid)
                .Select(obj => obj.Path)
                .FirstOrDefaultAsync();
        }
        else
        {
            path = await _garContext.AsAdmHierarchies.Where(obj => obj.Objectid == houseObject.Objectid)
                .Select(obj => obj.Path)
                .FirstOrDefaultAsync();
        }

        var pathArray = path?.Split('.').Select(int.Parse).ToList();
        var result = new List<AddressDTO>();
        var houseIndex = 0;
        for (var i = 0; i < pathArray.Count; i++)
        {
            var address = await _garContext.AsAddrObjs
                .FirstOrDefaultAsync(obj => obj.Objectid == pathArray[i]);
            if (address == null)
            {
                houseIndex = i;
                break;
            }

            var addressDto = new AddressDTO
            {
                ObjectId = address.Objectid,
                ObjectGuid = address.Objectguid,
                Text = $"{address.Typename} {address.Name}",
                Level = GarExtensions.ToGarAddressLevel(address.Level).ToString(),
                ObjectLevelText = GarExtensions.ObjectLevelToString(GarExtensions.ToGarAddressLevel(address.Level))
            };
            result.Add(addressDto);
        }

        for (var i = houseIndex; i < pathArray.Count; i++)
        {
            var house = await _garContext.AsHouses
                .FirstOrDefaultAsync(obj => obj.Objectid == pathArray[i]);
            if (house == null) continue;

            var addressDto = new AddressDTO
            {
                ObjectId = house.Objectid,
                ObjectGuid = house.Objectguid,
                Text = house.HouseFullNum,
                Level = GarExtensions.ToGarAddressLevel("10").ToString(),
                ObjectLevelText = GarExtensions.ObjectLevelToString(GarExtensions.ToGarAddressLevel("10"))
            };

            result.Add(addressDto);
        }

        return result;
    }
}
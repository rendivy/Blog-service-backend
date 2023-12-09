using blog_backend.DAO.Utils;
using blog_backend.Entity;

namespace blog_backend.Service.Extensions;

public static class GarExtensions
{

    public static GarAddressLevel ToGarAddressLevel(string level)
    {
        return level switch
        {
            "1" => GarAddressLevel.Region,
            "2" => GarAddressLevel.AdministrativeArea,
            "3" => GarAddressLevel.MunicipalArea,
            "4" => GarAddressLevel.RuralUrbanSettlement,
            "5" => GarAddressLevel.City,
            "6" => GarAddressLevel.Locality,
            "7" => GarAddressLevel.ElementOfPlanningStructure,
            "8" => GarAddressLevel.ElementOfRoadNetwork,
            "9" => GarAddressLevel.Land,
            "10" => GarAddressLevel.Building,
            "11" => GarAddressLevel.Room,
            "12" => GarAddressLevel.RoomInRooms,
            "13" => GarAddressLevel.AutonomousRegionLevel,
            "14" => GarAddressLevel.IntracityLevel,
            "15" => GarAddressLevel.AdditionalTerritoriesLevel,
            "16" => GarAddressLevel.LevelOfObjectsInAdditionalTerritories,
            "17" => GarAddressLevel.CarPlace,
            _ => throw new InvalidOperationException($"Unknown level type: {level}")
        };
    }


    public static string ObjectLevelToString(GarAddressLevel expression)
    {
        return expression switch
        {
            GarAddressLevel.Region => "Субъект РФ",
            GarAddressLevel.AdministrativeArea => "Административный район",
            GarAddressLevel.MunicipalArea => "Муниципальный район",
            GarAddressLevel.RuralUrbanSettlement => "Сельское/городское поселение",
            GarAddressLevel.City => "Город",
            GarAddressLevel.Locality => "Населенный пункт",
            GarAddressLevel.ElementOfPlanningStructure => "Элемент планировочной структуры",
            GarAddressLevel.ElementOfRoadNetwork => "Элемент улично-дорожной сети",
            GarAddressLevel.Land => "Земельный участок",
            GarAddressLevel.Building => "Здание (сооружение)",
            GarAddressLevel.Room => "Помещение",
            GarAddressLevel.RoomInRooms => "Помещения в пределах помещения",
            GarAddressLevel.AutonomousRegionLevel => "Уровень автономного округа (устаревшее)",
            GarAddressLevel.IntracityLevel => "Уровень внутригородской территории (устаревшее)",
            GarAddressLevel.AdditionalTerritoriesLevel => "Уровень дополнительных территорий (устаревшее)",
            GarAddressLevel.LevelOfObjectsInAdditionalTerritories =>
                "Уровень объектов на дополнительных территориях (устаревшее)",
            GarAddressLevel.CarPlace => "Машино-место",
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, "Invalid address level.")
        };
    }

    
    
    private static string GetHouseType(int? houseType)
    {
        return houseType switch
        {
            1 => "влд.",
            2 => "д.",
            3 => "двлд.",
            4 => "гар.",
            5 => "зд.",
            6 => "шх.",
            7 => "cтр.",
            8 => "Сооружение",
            9 => "л.",
            10 => "к.",
            11 => "под.",
            12 => "кот.",
            13 => "пог.",
            14 => "онс.",
            _ => throw new ArgumentOutOfRangeException(nameof(houseType), houseType, "Invalid house type.")
        };
    }


    public static string GetHouseName(this AsHouse house)
    {
        var houseName = house.Housenum!;
        var houseType = GetHouseType(house.Housetype!);
        
        if (house.Addtype1 != null)
        {
            houseName += $" {houseType} ";
            houseName += $"{house.Addnum1}";
        }

        if (house.Addtype2 != null)
        {
            houseName += $" {houseType} ";
            houseName += $"{house.Addnum2}";
        }

        return houseName;
    }
}
using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;
using Manifold.InfrastructureLayer.DTOs;

namespace Manifold.InfrastructureLayer.Helpers;
public static class MapHelper
{
    public static Valve MapToEntity(ValveDto dto)
    {
        return new Valve(dto.Id);
    }

    public static SourceValve MapToEntity(SourceValveDto dto)
    {
        return new SourceValve(dto.Id, Enum.Parse<ContentTypes>(dto.Content, ignoreCase: true));
    }

    public static WasteValve MapToEntity(WasteValveDto dto)
    {
        return new WasteValve(dto.Id) { IsHazardous = dto.IsHazardous };
    }

    public static Chamber MapToEntity(ChamberDto dto)
    {
        var chamber = new Chamber(dto.Id);

        chamber.Content = Enum.Parse<ContentTypes>(dto.Content);

        return chamber;
    }

}

using Manifold.DomainLayer.Enums;

namespace Manifold.ApplicationLayer.Services;
public interface IManifoldService
{
    void FillChamber(string chamberId, ContentTypes type);
    void EmptyChamber(string chamberId, bool hazardous);
}

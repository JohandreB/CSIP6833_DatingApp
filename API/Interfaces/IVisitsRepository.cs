using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IVisitsRepository
    {
        Task<UserVisit> GetUserVisit(int visiterUserId, int visitedUserId);
        Task<AppUser> GetUserWithVisits(int userId);
        Task<PagedList<LikeDto>> GetUserVisits(VisitParams likesParams);
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class VisitsRepository : IVisitsRepository
    {
        private readonly DataContext _context;
        public VisitsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserVisit> GetUserVisit(int visiterUserId, int visitedUserId)
        {
            return await _context.Visits.FindAsync(visiterUserId, visitedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserVisits(VisitParams visitsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();//all users
            var visits = _context.Visits.AsQueryable();//all visits
            DateTime viewDate = visitsParams.visitPeriod == "all" ? DateTime.MinValue.ToUniversalTime() : DateTime.UtcNow.AddDays(-30);

            if (visitsParams.Predicate == "visited")
            {
                visits = visits.Where(visit => visit.VisiterUserId == visitsParams.UserId
                    && visit.LastVisit > viewDate);//visits done by current user
                users = visits.Select(visit => visit.VisitedUser); // List of users visited by the current user
            }

            if (visitsParams.Predicate == "visitedBy")
            {
                visits = visits.Where(visit => visit.VisitedUserId == visitsParams.UserId
                    && visit.LastVisit > viewDate);//visits to current user
                users = visits.Select(visit => visit.VisiterUser); // List of users who have visited the current user
            }

            var visitUsers = users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });//either the users visited or users that visited the current user

            return await PagedList<LikeDto>.CreateAsync(visitUsers, visitsParams.PageNumber, visitsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithVisits(int userId)
        {
            return await _context.Users.Include(u => u.VisitedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
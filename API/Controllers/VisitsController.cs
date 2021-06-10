using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class VisitsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public VisitsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddVisit(string username)
        {
            var visiterUserId = User.GetUserId();
            var visitedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var visiterUser = await _unitOfWork.VisitsRepository.GetUserWithVisits(visiterUserId);

            var userVisit = await _unitOfWork.VisitsRepository.GetUserVisit(visiterUserId, visitedUser.Id);

            if (userVisit != null)
            {
                userVisit.LastVisit = System.DateTime.UtcNow;
            }
            else
            {
                userVisit = new UserVisit
                {
                    VisiterUserId = visiterUserId,
                    VisitedUserId = visitedUser.Id,
                    LastVisit = System.DateTime.UtcNow
                };

                visiterUser.VisitedUsers.Add(userVisit);
            }

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to add visit.");
        }

        [Authorize(Policy = "RequireVIPRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserVisits([FromQuery] VisitParams visitsParams)
        {
            visitsParams.UserId = User.GetUserId();
            var users = await _unitOfWork.VisitsRepository.GetUserVisits(visitsParams);//current logged in user

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }

    }
}
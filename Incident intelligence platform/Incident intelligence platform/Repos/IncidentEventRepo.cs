using Incident_intelligence_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Repos
{
    public class IncidentEventRepo
    {
        private readonly AppDbcontext appDbcontext;

        public IncidentEventRepo(AppDbcontext appDbcontext)
        {
            this.appDbcontext = appDbcontext;
        }
        public async Task<IEnumerable<IncidentEvent>> GetIncidentTimeLineAsync(int incidentId, int pageSize, int pageNumber)
        {
            var en = await appDbcontext.IncidentEvents
           .AsNoTracking()
           .Where(i => i.Id == incidentId)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            return en;

        }


        public async Task AddEvent(IncidentEvent newEvent)
        {
            await appDbcontext.IncidentEvents.AddAsync(newEvent);
        }


        public async Task<bool> CheckIncidentExist(int incidentId)
        {
            return await appDbcontext.Incidents.AnyAsync(i => i.Id == incidentId);
        }
    }
}//Get All Events For Spicfic Incident , Add Event

//  
/* 
  1 -Create Model done
  2- Set Relations done
  3- Add Model To DbContext done
  4- Create Repo to Deal with this Model done
  5- Create Service , Controller to Deal with it
  6-Create End Points Dtos 
  7- Register Repo and Service in Program.cs

Required Endpoints : 

  GET /api/incidents/{id}/timeline  
  POST /api/incidents/{id}/events
*/


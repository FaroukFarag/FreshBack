using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.AccessLevels;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Accounts;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Schedules;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Sites;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Users;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;

public interface IAirfobService :
    IAirfobSiteService,
    IAirfobScheduleService,
    IAirfobAccessLevelService,
    IAirfobFloorLevelService,
    IAirfobUserService,
    IAirfobAccountService
{
}

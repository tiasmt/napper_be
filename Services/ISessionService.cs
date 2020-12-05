using System;
using napper_be.Models;
using napper_be.Repository;

namespace napper_be.Services
{
    public interface ISessionService
    {

        Session Open(int userId);

        Session Update(string sessionId);
        

        void Close(string sessionGUID);
    }

}
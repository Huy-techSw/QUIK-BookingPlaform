using Microsoft.EntityFrameworkCore;
using Quik_BookingApp.DAO;
using Quik_BookingApp.DAO.Models;
using Quik_BookingApp.Repos.Interface;
using System.Security.Cryptography;

namespace Quik_BookingApp.Service
{
    public class RefreshHandler : IRefreshHandler
    {
        private readonly QuikDbContext context;
        public RefreshHandler(QuikDbContext context)
        {
            this.context = context;
        }
        public async Task<string> GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string refreshtoken = Convert.ToBase64String(randomnumber);
                var Existtoken = context.TblRefreshtokens.FirstOrDefaultAsync(item => item.UserId == username).Result;
                if (Existtoken != null)
                {
                    Existtoken.RefreshToken = refreshtoken;
                }
                else
                {
                    await context.TblRefreshtokens.AddAsync(new TblRefreshToken
                    {
                        UserId = username,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken = refreshtoken
                    });
                }
                await context.SaveChangesAsync();

                return refreshtoken;

            }
        }
    }
}

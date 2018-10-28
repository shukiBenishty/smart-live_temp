using BE.Events;
using BE.Models;
using DAL.DB;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public class Login
    {

        private SmartLifeDbContext db { get; }

        private IEventAggregator _eventAggregator;

        public Login(IEventAggregator eventAggregator)
        {
            db = new SmartLifeDbContext();
            _eventAggregator = eventAggregator;
        }

        public async Task<bool> GetIsLogOn()
        {
            var currentAccount = await db.CurrentAccount.FirstOrDefaultAsync();
            if (currentAccount == null || currentAccount.AccountID == new Guid())
            {
                return false;
            }

            CheckIsResiter(currentAccount);
            return true;
        }

        private void CheckIsResiter(CurrentAccount currentAccount)
        {
            (new Thread(() =>
            {
                var account = db.Accounts.Where(A => A.Id == currentAccount.Id).SingleOrDefault();
                if (account == null)
                {
                    return;
                }
                if (account.Profile == null)
                {
                    _eventAggregator.GetEvent<OpenRegisterModeEvent>().Publish();
                }
            })).Start();
        }

        public async Task<Guid> GetAccountId()
        {
            var currentProfile = await db.CurrentAccount.FirstOrDefaultAsync();
            if (currentProfile == null)
            {
                return new Guid();
            }
            return currentProfile.AccountID;
        }

        public async Task UserLogOut(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<BE.Events.UserLogoutEvent>().Publish();
            var currentProfile = await db.CurrentAccount.FirstOrDefaultAsync();
            if (currentProfile == null)
            {
                return;
            }
            currentProfile.AccountID = new Guid();
            db.CurrentAccount.AddOrUpdate(currentProfile);
            db.SaveChanges();

        }

        public async Task<bool> TryLoginAsync(String user, String pass)
        {
            var account = await
                (db.Accounts.Where(U => U.Email == user && U.Password == pass)).FirstOrDefaultAsync();
            if (account == null || account.Id == null ||account.Id == new Guid())
            {
                return false;
            }
            await UpdateCurrentAccount(account.Id);
            if (account.Profile == null)
            {
                _eventAggregator.GetEvent<OpenRegisterModeEvent>().Publish();
            }
            return true;

        }

        private async Task UpdateCurrentAccount(Guid accountId)
        {
            var current = await db.CurrentAccount.FirstOrDefaultAsync();
            if (current == null)
            {
                current = new CurrentAccount();
            }
            current.AccountID = accountId;
            db.CurrentAccount.AddOrUpdate(current);
            await db.SaveChangesAsync();
        }

        public async Task<Boolean> TryRegister(Account account)
        {
            var result = await db.Accounts.Where(A => A.Email == account.Email || A.GoogleSub != "" && A.GoogleSub == account.GoogleSub).SingleOrDefaultAsync();
            if (result != null)
            {
                return false;
            }
            db.Accounts.AddOrUpdate(A => A.Id, account);
            await db.SaveChangesAsync();
            await UpdateCurrentAccount(account.Id);
            return true;

        }

        public async Task TryGoogleLoginAsync(Dictionary<string, string> userDetails, IEventAggregator eventAggregator)
        {
            await UserLogOut(eventAggregator);
            var googleSub = userDetails["sub"];
            var account =  db.Accounts.Where(A => A.GoogleSub == googleSub).Select(A=>A).SingleOrDefault();
            if (account == null)
            {
                eventAggregator.GetEvent<OpenRegisterModeEvent>().Publish();
                account = new BE.Models.Account()
                {
                    GoogleSub = userDetails["sub"],
                    GoogleProfile = new GoogleProfile()
                    {

                        DisplayName = userDetails["name"],
                        GooglePlus = userDetails["profile"],
                        Picture = userDetails["picture"]

                    },
                    Profile = new Profile()
                    {
                        FiestName = userDetails["given_name"],
                        LastName = userDetails["family_name"],
                    }
                };
            }
            else if(account.Profile == null)
            {
                eventAggregator.GetEvent<OpenRegisterModeEvent>().Publish();
            }
            db.Accounts.AddOrUpdate(A => A.Id, account);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;
            var eventArg = new UserLogInSeccEventArg();

            eventArg.Id = account.Id;
            eventArg.Name = userDetails["name"];
            eventArg.GivenName = userDetails["given_name"];
            eventArg.FamilyName = userDetails["family_name"];
            eventArg.GooglePlus = userDetails["profile"];
            eventArg.Picture = userDetails["picture"];
            
            eventAggregator.GetEvent<UserLogInSeccEvent>().
                 Publish(eventArg);
            await UpdateCurrentAccount(account.Id);
           

        }

        public async Task<bool> GetisRegister()
        {
            var currentAccount = await db.CurrentAccount.FirstOrDefaultAsync();
            if (currentAccount == null || currentAccount.AccountID == new Guid())
            {
                return false;
            }
           var account = await  db.Accounts.Where(A => A.Id == currentAccount.Id).SingleAsync();
            if (account.Profile == null)
            {
                return true;
            }
           
            return false;
        }
    }


}

using devinmajordotcom.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace devinmajordotcom.Models
{
    public partial class dbContext
    {

        private static void dbContextStaticPartial() { }

        public override int SaveChanges()
        {
            var dataService = new BaseDataService();
            var userName = "";
            var userGuid = HttpContext.Current.Session["MainPageUserAuthID"];
            if (userGuid == null)
            {
                userName = "Default";
            }
            else
            {
                userName = dataService.GetCurrentUser((Guid)userGuid).UserName;
            }

            var addedAuditedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Added).Select(p => p.Entity);
            var modifiedAuditedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).Select(p => p.Entity);

            const string _createdOn = "CreatedOn";
            const string _createdBy = "CreatedBy";
            const string _modifiedOn = "ModifiedOn";
            const string _modifiedBy = "ModifiedBy";


            foreach (var added in addedAuditedEntities)
            {
                var createdOn = added.GetType().GetProperty(_createdOn);
                if (createdOn != null)
                {
                    createdOn.SetValue(added, DateTime.Now, null);
                }
                var createdBy = added.GetType().GetProperty(_createdBy);
                if (createdBy != null)
                {
                    createdBy.SetValue(added, userName, null);
                }
                var modifiedOn = added.GetType().GetProperty(_modifiedOn);
                if (modifiedOn != null)
                {
                    modifiedOn.SetValue(added, DateTime.Now, null);
                }
                var modifiedBy = added.GetType().GetProperty(_modifiedBy);
                if (modifiedBy != null)
                {
                    modifiedBy.SetValue(added, userName, null);
                }
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                var modifiedOn = modified.GetType().GetProperty(_modifiedOn);
                if (modifiedOn != null)
                {
                    modifiedOn.SetValue(modified, DateTime.Now, null);
                }
                var modifiedBy = modified.GetType().GetProperty(_modifiedBy);
                if (modifiedBy != null)
                {
                    modifiedBy.SetValue(modified, userName, null);
                }
            }

            return base.SaveChanges();

        }



    }
}
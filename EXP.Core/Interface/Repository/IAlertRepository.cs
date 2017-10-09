using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;
using EXP.Entity.Enumerations;

namespace EXP.Core.Interface
{
    public interface IAlertRepository
    {
        void CreateAlert(Alert alert);
        List<Alert> GetActiveAlertsForUser(int userId);
        List<Alert> GetAlertsForUserByEntity(int userId, AlertEntityTypeEnum entityType, int entityId);
        void DeactivateAlert(int alertId);
        void ActivateAlert(int alertId);
    }
}

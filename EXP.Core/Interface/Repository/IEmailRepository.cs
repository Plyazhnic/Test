using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IEmailRepository
    {
        void CreateEmail(Email email);
        Email GetNextEmail();
        void SetEmailSent(Email email);
    }
}
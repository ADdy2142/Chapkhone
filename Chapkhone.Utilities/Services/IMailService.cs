using Chapkhone.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.Utilities.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestVM request);
    }
}
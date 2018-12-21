using System;

namespace SimplySecureApi.Data.Models.Domain.ViewModels
{
    public class ModuleViewModel : BaseComponentViewModel
    {
        public Guid Id { get; set; }

        public DateTime LastHeartbeat { get; set; }
    }
}
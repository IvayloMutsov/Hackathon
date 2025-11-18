using Infrastructure.Models;

namespace Infrastructure.ViewModels
{
    public class ProcedureViewModel
    {
        public Procedures Procedure { get; set; }

        public List<Professors> Professors { get; set; }
    }
}

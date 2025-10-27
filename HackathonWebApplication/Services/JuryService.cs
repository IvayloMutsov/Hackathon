using Data;
using Infrastructure.Models;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class JuryService
    {
        private readonly AppDbContext context;

        public JuryService(AppDbContext context)
        {
            this.context = context;
        }

        public List<Proffessors> GetJury(string scienceField, Procedures procedure)
        {
            var all = context.Proffessors.Where(a => a.ConsecutiveCounter < 2 &&
                                                a.ScientificFiled == scienceField &&
                                                (a.LastParticipationDate == procedure.Date)).ToList();
            var local = context.Proffessors.Where(l => l.UniIsLocal == true).ToList();
            var external = context.Proffessors
                .Where(l => l.UniIsLocal == false)
                .OrderByDescending(l => l.Distance)
                .ToList();

            int totalMembers = procedure.ProcedureType switch
            {
                "доктор" => 5,
                "доцент" => 7,
                "доктор на науките" or "ДН" => 7,
                "професор" => 7,
                _ => 7
            };

            int minProf = procedure.ProcedureType switch
            {
                "доктор" => 1,
                "доцент" => 3,
                "доктор на науките" or "ДН" => 3,
                "професор" => 4,
                _ => 3
            };

            int minExt = procedure.ProcedureType switch
            {
                "доктор" => 3,
                "доцент" => 3,
                "доктор на науките" or "ДН" => 4,
                "професор" => 3,
                _ => 3
            };

            var selected = new List<Proffessors>();

            selected.AddRange(external.Take(minProf));

            int profs = selected.Count(p => p.AcademicRank == "професор");

            if (profs < minProf)
            {
                var missing = minProf - profs;
                var more = all
                    .Where(p => p.AcademicRank == "професор" && !selected.Contains(p))
                    .Take(missing);
                selected.AddRange(more);
            }

            foreach (var p in all)
            {
                if (selected.Count >= totalMembers)
                    break;
                if (!selected.Contains(p))
                    selected.Add(p);
            }

            var reserved = new List<Proffessors>();
            var extReserve = external.FirstOrDefault(p => !selected.Contains(p));
            var intReserve = local.FirstOrDefault(p => !selected.Contains(p));

            if (intReserve != null)
            {
                reserved.Add(intReserve);
            }

            if (extReserve != null)
            {
                reserved.Add(extReserve);
            }

            foreach(var item in reserved)
            {
                selected.Add(item);
            }

            return selected;
        }
    }
}

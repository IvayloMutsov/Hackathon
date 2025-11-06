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
            // 1️⃣ Извличаме всички хабилитирани в съответната област
            var professors = context.Proffessors
                .Where(p => p.ScientificFiled == scienceField &&
                            p.ConsecutiveCounter < 2)
                .ToList();

            var local = professors.Where(p => p.UniIsLocal).ToList();
            var external = professors.Where(p => !p.UniIsLocal)
                                     .OrderBy(p => p.Distance)
                                     .ToList();

            // 2️⃣ Настройки според типа процедура
            int totalMembers, minProf, minExt;

            switch (procedure.ProcedureType)
            {
                case "доктор":
                    totalMembers = 5;
                    minProf = 1;
                    minExt = 3;
                    break;
                case "доктор на науките":
                case "ДН":
                    totalMembers = 7;
                    minProf = 3;
                    minExt = 4;
                    break;
                case "доцент":
                    totalMembers = 7;
                    minProf = 3;
                    minExt = 3;
                    break;
                case "професор":
                    totalMembers = 7;
                    minProf = 4;
                    minExt = 3;
                    break;
                default:
                    totalMembers = 7;
                    minProf = 3;
                    minExt = 3;
                    break;
            }

            // 3️⃣ Избираме минималния брой външни членове (най-близките)
            var selected = new List<Proffessors>();
            selected.AddRange(external.Take(minExt));

            // 4️⃣ Проверяваме дали има достатъчно професори
            int currentProfessors = selected.Count(p => p.AcademicRank == "професор");

            if (currentProfessors < minProf)
            {
                int missing = minProf - currentProfessors;
                var extraProfs = professors
                    .Where(p => p.AcademicRank == "професор" &&
                                !selected.Any(s => s.ID == p.ID))
                    .Take(missing);
                selected.AddRange(extraProfs);
            }

            // 5️⃣ Попълваме останалите места до общия брой
            foreach (var p in professors)
            {
                if (selected.Count >= totalMembers)
                    break;

                if (!selected.Any(s => s.ID == p.ID))
                    selected.Add(p);
            }

            // 6️⃣ Добавяме двама резервни – един вътрешен и един външен
            var internalReserve = local.FirstOrDefault(p => !selected.Any(s => s.ID == p.ID));
            var externalReserve = external.FirstOrDefault(p => !selected.Any(s => s.ID == p.ID));

            if (internalReserve != null)
                selected.Add(internalReserve);

            if (externalReserve != null)
                selected.Add(externalReserve);

            // 7️⃣ Обновяваме ConsecutiveCounter:
            // участниците +1, останалите = 0
            foreach (var p in professors)
            {
                if (selected.Any(s => s.ID == p.ID))
                    p.ConsecutiveCounter++;
                else
                    p.ConsecutiveCounter = 0;
            }

            context.SaveChanges();

            // Връщаме целия списък (основни + 2 резервни)
            return selected;
        }
    }
}

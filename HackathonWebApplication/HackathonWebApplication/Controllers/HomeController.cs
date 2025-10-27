using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Services;
using Infrastructure.ViewModels;
using Data;
using Microsoft.EntityFrameworkCore;

namespace HackathonWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly JuryService _juryService;

    public HomeController(ILogger<HomeController> logger, JuryService juryServices, AppDbContext context)
    {
        _logger = logger;
        _juryService = juryServices;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var procedures = await _context.Procedures.ToListAsync();
        var professors = await _context.Proffessors.ToListAsync();

        var viewModel = procedures.Select(p => new ProcedureViewModel
        {
            Procedure = p,
            Professors = professors
                .Where(prof =>
                    prof.ID == p.ProfessorId1 ||
                    prof.ID == p.ProfessorId2 ||
                    prof.ID == p.ProfessorId3 ||
                    prof.ID == p.ProfessorId4 ||
                    prof.ID == p.ProfessorId5 ||
                    prof.ID == p.ProfessorId6 ||
                    prof.ID == p.ProfessorId7 ||
                    prof.ID == p.ReserveInternalId ||
                    prof.ID == p.ReserveExternalId)
                .ToList()
        }).ToList();

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> CreateProcedure(Procedures dto,string field)
    {
        var allProffesors = _context.Proffessors.ToList();
        var viewModel = _juryService.GetJury(field,dto);

        var selectedIds = viewModel.Select(m => m.ID).ToHashSet();

        foreach (var prof in allProffesors)
        {
            if (prof.LastParticipationDate != prof.PrevParticipationDate)
            {
                if (selectedIds.Contains(prof.ID) && prof.ConsecutiveCounter < 2)
                {
                    prof.ConsecutiveCounter += 1;
                    prof.PrevParticipationDate = prof.LastParticipationDate;
                    prof.LastParticipationDate = DateTime.Now;
                }
                else
                {
                    prof.ConsecutiveCounter = 0;
                }
            }
            _context.Update(prof);
        }

        var procedure = new Procedures
        {
            Date = dto.Date,
            ProcedureType = dto.ProcedureType,

            ProfessorId1 = (int)(viewModel.ElementAtOrDefault(0)?.ID),
            ProfessorId2 = (int)(viewModel.ElementAtOrDefault(1)?.ID),
            ProfessorId3 = (int)(viewModel.ElementAtOrDefault(2)?.ID),
            ProfessorId4 = (int)(viewModel.ElementAtOrDefault(3)?.ID),
            ProfessorId5 = (int)(viewModel.ElementAtOrDefault(4)?.ID),
            ProfessorId6 = (int)viewModel.ElementAtOrDefault(5)?.ID,
            ProfessorId7 = (int)viewModel.ElementAtOrDefault(6)?.ID,

            ReserveInternalId = (int)(viewModel.ElementAtOrDefault(0)?.ID),
            ReserveExternalId = (int)(viewModel.ElementAtOrDefault(1)?.ID),
        };

        await _context.Procedures.AddAsync(procedure);
        await _context.SaveChangesAsync();

        return View(new ProcedureViewModel
        {
            Procedure = procedure,
            Professors = viewModel
        });
    }
}

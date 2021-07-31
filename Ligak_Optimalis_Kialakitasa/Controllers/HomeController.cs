using Microsoft.AspNetCore.Mvc;
using Ligak_Optimalis_Kialakitasa.Models;
using Ligak_Optimalis_Kialakitasa.Repository;
using Ligak_Optimalis_Kialakitasa.Models.Validation;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ligak_Optimalis_Kialakitasa.Controllers
{
    public class HomeController : Controller
    {
        private TournamentRepository tournamentRepository;

        public HomeController(TournamentRepository tcr)
        {
            this.tournamentRepository = tcr;
        }

        [ResponseCache(Duration = 30)]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration =30)]
        public PartialViewResult Header()
        {
            return PartialView("Header");
        }

        [ResponseCache(Duration = 30)]
        public PartialViewResult ResultPartial()
        {
            return PartialView("ResultPartial");
        }

        public PartialViewResult Footer()
        {
            return PartialView("Footer");
        }

        [HttpGet]
        public IActionResult ConstraintsAndRules()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConstraintsAndRules(TournamentConstraintsAndRules tcr)
        {
            var robin = Request.Form["Robin"];
            tcr.Robins = (robin == "Single Round Robin" ? Robins.Single_Round_Robin : Robins.Double_Round_Robin);
            tournamentRepository.Tournament = new Tournament(ref tcr);
            tournamentRepository.TournamentConstraintsRules = tcr;
            return RedirectToAction(nameof(Teams));
        }

        [HttpGet]
        public IActionResult Teams()
        {
            return View(tournamentRepository.Tournament);
        }

        [HttpPost]
        public async Task<IActionResult> Teams(Tournament tournament)
        {
            IFormCollection ic = await HttpContext.Request.ReadFormAsync();
            if (!TeamsValidator.Validate(ref ic, ref tournamentRepository))
            {
                return View(tournamentRepository.Tournament);
            }
            tournamentRepository.Tournament.NameOfTournament = tournament.NameOfTournament;
            return RedirectToAction(nameof(Generate));
        }



        [HttpGet]
        public IActionResult Generate()
        {
            return View(tournamentRepository.Tournament);
        }


        [HttpGet]
        public IActionResult Result()
        {
            Result result = ResultGeneratorLogic.Solve(tournamentRepository.Tournament, tournamentRepository.TournamentConstraintsRules);
            return View(result);
        }

        [HttpPost]
        public IActionResult Optimize()
        {
            var optimize_mode = Request.Form["optimize"];
            Result result = ResultGeneratorLogic.Solve(optimize_mode);
            return View(result);
        }
    }
}

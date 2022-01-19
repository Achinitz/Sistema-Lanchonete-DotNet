using Microsoft.AspNetCore.Mvc;
using LanchesMac.ViewModels;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;

namespace LanchesMac.Controllers
{
    public class LancheController : Controller
    {

        private readonly ILancheRepository _lancheRepository;

        public LancheController(ILancheRepository lancheRepository)
        {
            _lancheRepository = lancheRepository;
        }

       /* public IActionResult List()
        {
            
            var lanches = _lancheRepository.Lanches;
            var totalLanches = lanches.Count();

            ViewData["Titulo"] = "Todos os Lanches";
            ViewData["Data"] = DateTime.Now;

            ViewBag.Total = "Total de lanches:";
            ViewBag.TotalLanches = totalLanches;

            var lanchesListViewModel = new LancheListViewModel(){
                Lanches = _lancheRepository.Lanches,
                CategoriaAtual = "Categoria Atual"
            };

            return View(lanchesListViewModel);
        }*/

        public IActionResult List(string categoria)
        {
            IEnumerable<Lanche> lanches;

            string categoriaAtual = string.Empty;

            if(string.IsNullOrEmpty(categoria))
            {
                lanches = _lancheRepository.Lanches.OrderBy(l=>l.LancheId);
                categoriaAtual = "Todos os lanches";
            }else
            {
                lanches = _lancheRepository.Lanches.Where(l => l.Categoria.CategoriaNome.Equals(categoria)).OrderBy(c => c.Nome);
            }

            var lancheListViewModel = new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };

            return View(lancheListViewModel);
        }

        public IActionResult Details(int lancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(l=> l.LancheId == lancheId);

            return View(lanche);
        }

        public ViewResult Search(string searchString)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if(string.IsNullOrEmpty(searchString))
            {
                lanches = _lancheRepository.Lanches.OrderBy(p=>p.LancheId);
                categoriaAtual = "Todos os Lanches";
            }else
            {
                lanches = _lancheRepository.Lanches.Where(p=>p.Nome.ToLower().Contains(searchString.ToLower()));

                if(lanches.Any())
                {
                    categoriaAtual = "Lanches";
                }else
                {
                    categoriaAtual = "Nenhum lanche foi encontrado";
                }
            }
            return View("~/Views/Lanche/List.cshtml",new LancheListViewModel{
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            });
        }

    }
}
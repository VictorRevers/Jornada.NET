using Microsoft.AspNetCore.Mvc;
using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistence;
using DevTrackR.API.Persitence.Repository;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DevTrackR.API.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _repository;
        private readonly ISendGridClient _client;
        public PackagesController(IPackageRepository repository, ISendGridClient client)
        {
            _repository = repository;
            _client = client;
        }

        //GET /api/packages
        [HttpGet]
        public IActionResult GetAll()
        {
            var packages = _repository.GetAll();
            
            return Ok(packages);
        }

         //GET /api/packages/{code}
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code)
        {
            var package = _repository.GetByCode(code);

            if(package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

         //POST /api/packages
        /// <summary>
        /// Cadastro de um pacote.
        /// </summary>
        /// <remarks>
        /// {
        ///     "title": "Titulo",
        ///     "weight": 2.5,
        ///     "senderName": "Nome",
        ///     "senderEmail": "seu@email.com"
        /// }
        /// </remarks>
        /// <param name="model">Dados do pacote</param>
        /// <returns>Objeto recém criado</returns>
        /// <response code ="201">Cadastro realizado com sucesso.</response>
        /// <response code ="400">Dados Inválidos</response>

        [HttpPost]
        public async Task<IActionResult> Post(AddPackageInputModel model)
        {
            if(model.Title.Length < 10)
            {
                return BadRequest("Title length must be at least 10 characters long.");
            }
            var package = new Package(model.Title, model.Weight);

           _repository.Add(package);

           var message = new SendGridMessage{
               From = new EmailAddress("2020101175@aluno.unicarioca.edu.br" , "victorrk"),
               Subject = "Your package was dispatched.",
               PlainTextContent = $"Your package {package.Code} was dispatched."
           };

           message.AddTo(model.SenderEmail, model.SenderName);
           await _client.SendEmailAsync(message);

            return CreatedAtAction(
                "GetByCode", 
                new{code = package.Code}, 
                package);
        }

         //POST /api/packages/{code}/updates
        [HttpPost("{code}/updates")]
        public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model)
        {
            var package = _repository.GetByCode(code);
            if(package == null)
            {
                return NotFound();
            }
            package.AddUpdate(model.Status, model.Delivered);
            
            _repository.Update(package);
            return NoContent();

        }
    }
}
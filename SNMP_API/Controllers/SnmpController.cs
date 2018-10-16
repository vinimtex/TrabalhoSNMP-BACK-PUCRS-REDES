using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SNMP_API.Models;

namespace SNMP_API.Controllers
{
    [Route("api/[controller]")]
    public class SnmpController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /*
         * Ponto de Entreda:
         * recebe a Community e Endereço do Agente a ser Monitorado
         * deve iniciar todos os serviços e libera o uso de outras rotas
         * para o front end consumir
        */
        [HttpPost("start")]
        public IActionResult Start([FromBody] Setting setting)
        {
            return Json(setting);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

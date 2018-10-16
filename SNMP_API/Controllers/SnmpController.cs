using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SNMP_API.Models;
using SnmpSharpNet;

namespace SNMP_API.Controllers
{
    [Route("api/[controller]")]
    public class SnmpController : Controller
    {
        // GET: api/snpmget
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
            OctetString community = new OctetString(setting.Community);

            AgentParameters param = new AgentParameters(community);

            param.Version = SnmpVersion.Ver2;

            IpAddress agent = new IpAddress(setting.AgentAddress);

            UdpTarget target = new UdpTarget((System.Net.IPAddress)agent, 161, 2000, 1);

            Pdu pdu = new Pdu(PduType.Get);

            pdu.VbList.Add("1.3.6.1.2.1.1.5.0"); //sysName

            SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);

            // If result is null then agent didn't reply or we couldn't parse the reply.
            if (result != null)
            {
                // ErrorStatus other then 0 is an error returned by 
                // the Agent - see SnmpConstants for error definitions
                if (result.Pdu.ErrorStatus != 0)
                {
                    // agent reported an error with the request
                    return Json(("Error in SNMP reply. Error {0} index {1}",
                        result.Pdu.ErrorStatus,
                        result.Pdu.ErrorIndex));
                }
                else
                {
                    return Json(("sysName({0}) ({1}): {2}",
                        result.Pdu.VbList[4].Oid.ToString(),
                        SnmpConstants.GetTypeName(result.Pdu.VbList[4].Value.Type),
                        result.Pdu.VbList[4].Value.ToString()));
                }
            }
            else
            {
                return Json("No response received from SNMP agent.");
            }
            target.Close();

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

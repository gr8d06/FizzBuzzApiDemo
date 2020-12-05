using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FizzBuzz.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FizzBuzz.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        // GET: /<LogsController>
        [HttpGet]
        public IActionResult Get()
        {
            var logSet = QuickDatabase.GetLogs();

            return new JsonResult(logSet);
        }

        // GET /<LogsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var log = QuickDatabase.GetLogById(id);
            if(!log.ID.HasValue)
            {
                return NotFound();
            }
            return new JsonResult(log);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FizzBuzz.Repository;
using FizzBuzz.Models;

namespace FizzBuzz.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FizzBuzzController : ControllerBase
    {
        private const int defaultTopNum = 100;


        [HttpGet]
        public IActionResult GetFizzBuzz()
        {
            try
            {
                var output = ExecuteFizzBuzz(defaultTopNum);
                //QuickLogger(200, output);
                return new JsonResult(output);
            }
            catch(Exception e)
            {
                //QuickLogger(500, e.Message);
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
                        
        }

        [HttpGet("{topNumber}")]
        public IActionResult GetFizzBuzz(int topNumber)
        {
            if(topNumber < 1 || topNumber > 1000)
            {
                string response = "This is only designed to handle numbers between 1 and 1000.";
                //QuickLogger(400, response);
                return BadRequest(response);
            }
            var output = ExecuteFizzBuzz(topNumber);
            //QuickLogger(200, output);
            return new JsonResult(output);
        }

        [HttpPost]
        public IActionResult PostFizzBuzz([FromBody]int topNumber)
        {
            if (topNumber < 1 || topNumber > 1000)
            {
                var response = "This is only designed to handle numbers between 1 and 1000.";
                //QuickLogger(400, response);
                return BadRequest(response);
            }
            var output = ExecuteFizzBuzz(topNumber);
            //QuickLogger(200, output);
            return new JsonResult(output);
        }


        private string ExecuteFizzBuzz(int topNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            string output = "";

            for (int i = 1; i <= topNumber; i++)
            {
                if ((i % 3 != 0) && (i % 5 != 0))
                {
                    sb.Append(i + ", ");
                    continue;
                }

                if (i % 3 == 0)
                {
                    output = "Fizz";
                }

                if (i % 5 == 0)
                {
                    output += "Buzz";
                }


                sb.Append(output);
                
                if (i != topNumber)
                    sb.Append(", ");
                else
                    sb.Append("}");

                output = "";
            }

            return sb.ToString();
        }

        private void QuickLogger(int statusCode, string result)
        {
            QuickDatabase.WriteFbLog(new FizzBuzzLogDto { LogDate = DateTime.Now, StatusCode = statusCode, Results = result });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FizzBuzz.Models
{
    public class FizzBuzzLogDto
    {
        public FizzBuzzLogDto()
        { }

        public int? ID { get; set; }
        public DateTime LogDate { get; set; }
        public string Results { get; set; }
        public int StatusCode { get; set; }
    }
}

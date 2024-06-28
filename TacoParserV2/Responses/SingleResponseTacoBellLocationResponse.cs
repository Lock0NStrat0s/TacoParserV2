using Sprache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacoParserV2.Responses;

public class SingleResponseTacoBellLocationResponse
{
        public List<Result> results { get; set; }
        public Status status { get; set; }

    public class Result
    {

        public string formatted { get; set; }
        public Geometry geometry { get; set; }
    }
    public class Status
    {
        public int code { get; set; }
        public string message { get; set; }
    }
    public class Geometry
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}

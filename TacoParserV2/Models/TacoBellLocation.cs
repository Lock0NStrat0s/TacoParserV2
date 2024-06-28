using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacoParserV2.Models;

public class TacoBellLocation : ITrackable
{
    public string Name { get; set; }
    public Point Location { get; set; }
}

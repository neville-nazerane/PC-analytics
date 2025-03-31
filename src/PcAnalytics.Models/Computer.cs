﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcAnalytics.Models
{

    [Index(nameof(Identifier))]
    public class Computer
    {

        public int Id { get; set; }

        public required string Identifier { get; set; }

        public required string FriendlyName { get; set; }

        public IEnumerable<Record>? Records { get; set; }

    }
}

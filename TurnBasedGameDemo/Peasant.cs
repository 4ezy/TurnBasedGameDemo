﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedGameDemo
{
    [Serializable]
    public class Peasant : Unit
    {
        public Peasant() 
            : base(10, 1, 2, 2, 1) { }
    }
}

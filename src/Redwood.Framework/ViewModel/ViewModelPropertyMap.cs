﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redwood.Framework.ViewModel
{
    public class ViewModelPropertyMap
    {
        public string Name { get; set; } 

        public ViewModelProtectionSettings ViewModelProtection { get; set; }

        public Type Type { get; set; }

        public bool TransferToServer { get; set; }

        public bool TransferToClient { get; set; }
    }
}

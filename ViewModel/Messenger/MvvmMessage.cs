using EngineeringThesis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringThesis.ViewModel
{
    class MvvmMessage
    {
        public int SelectedWindow { get; set; }
        public RootObject Data { get; set; }
    }
}

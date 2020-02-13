using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFun.Models.Bingo
{
    public class Square
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public bool CanCheck { get; set; }
        public bool IsWinner { get; set; }
    }
}

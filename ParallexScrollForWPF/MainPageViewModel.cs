using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallexScrollForWPF
{
    public class MainPageViewModel
    {
        public List<string> Items { get; private set; }

        public MainPageViewModel()
        {
            this.Items = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                Items.Add(i.ToString());
            }
        }
    }
}

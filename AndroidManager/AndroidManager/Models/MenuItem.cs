using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Models
{
    public class MenuItem
    {
        public MenuItem(string name, string displayName, string glyph)
        {
            Name = name;
            DisplayName = displayName;
            Glyph = glyph;
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Glyph { get; set; }
    }
}

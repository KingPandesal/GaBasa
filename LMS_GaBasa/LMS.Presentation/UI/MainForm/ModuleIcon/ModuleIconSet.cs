using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LMS.Presentation.UI.MainForm.ModuleIcon
{
    internal sealed class ModuleIconSet
    {
        public Image White { get; }
        public Image Red { get; }

        public ModuleIconSet(Image white, Image red)
        {
            White = white;
            Red = red;
        }
    }

}

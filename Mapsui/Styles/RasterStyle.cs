using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapsui.Styles
{
    public class RasterStyle : IStyle
    {
        /// <inheritdoc />
        public double MinVisible { get; set; }

        /// <inheritdoc />
        public double MaxVisible { get; set; }

        /// <inheritdoc />
        public bool Enabled { get; set; }

        /// <inheritdoc />
        public float Opacity { get; set; }
    }
}

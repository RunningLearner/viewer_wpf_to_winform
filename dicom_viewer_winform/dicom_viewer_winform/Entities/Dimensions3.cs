using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dicom_viewer_winform.Entities
{
    public class Dimensions3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vector3 AsVector()
        {
            return new Vector3(X, Y, Z);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.PowerUps
{
    public interface IUpgrade
    {
        public int Cost {  get; set; }
        public int Quantity { get; set; }
        public string UpgradeName { get; set; }
    }
}

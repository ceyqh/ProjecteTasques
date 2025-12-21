using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppTasques
{
    public class Tasca
    {
        public int id { get; set; }
        public string nom {  get; set; }
        public string descripcio { get; set; }
        public string etiqueta { get; set; }
        public string colorEtiqueta { get; set; }
        public string dataInici {  get; set; }
        public string dataFinal { get; set; }
        public string estat { get; set; }
        public string colorEstat { get; set; }
        public int usuariId { get; set; }
        public string usuariNom{ get; set; }
    }
}

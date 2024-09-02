using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon
{
    public class Bicho
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TypeInfo> Types { get; set; }    

        public Sprites Sprites { get; set; }
    
    }

    public class TypeInfo
    {
        public TypeDetalhe Type { get; set; }
    }

    public class TypeDetalhe
    {
        public string Name { get; set; }
    }

    public class Sprites
    {
        public string Front_default { get; set; }
    }


}

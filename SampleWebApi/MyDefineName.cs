using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi
{
    public class MyDefineName:Attribute
    {
        private string Name;
        public MyDefineName(string name)
        {
            Name = name;
          
        }
    }
}

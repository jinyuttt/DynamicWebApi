using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample
{
   
    public class MyAdd : ICall
    {
        protected const string BilName = "MM";
       
        public MyAdd()
        {

          //  BilName = "sss";
        }

      public  string Add()
        {
            return "成都";
        }

        public string UserServer()
        {
            return "银行名称";
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace Sample
{

    public class MyAdd : ICall
    {
        //protected const string BilName = "MM";
       
        public MyAdd()
        {

          //  BilName = "sss";
        }

        [HttpGet]
        public  string Add()
        {
            return "成都";
        }

        public string UserServer()
        {
            return "银行名称";
        }

        public ResponceKK GetAsyncResult(RequestKK request)
        {
            return new ResponceKK();
        }
    }
}

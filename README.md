# DynamicWebApi
动态创建WebApi  
将一般类快速部署为WEBRPC(webapi)

-------------------------------------------------

使用方法： 
1.安装库 DynamicControllersFactory  
2.在webapi服务中添加扩展  
  services.AddWebApiDirectory();//添加程序集  

            //添加服务扩展
            services.AddDynamicWebApi(new DynamicWebApiOptions()  
            {  
                ControllerFeature = (P) => {  

                    //注入判断为控制器的类，返回true就是控制器  
                    if (P.GetInterface(typeof(ICall).Name) == null)  
                    {
                        return false;  
                    }  
                    return true;  
                }  
            });  
3.可以添加版本信息，映射新的控制器URL名称和域名称  
    DynamicWebApiOptions有配置项，原理是根据配置值，查找该值的静态字段，常量，获取对应的值进行替换  
	例如：  
	 ControllerVersion = "vserion", 类常量字段  const string version = "v2";   
	 则路径规则变为：api/v2/控制器名称/方法名称，注意：配置的是字段名称
	 同理再例如：  
	  ControllerMapName = "MapName"，类常量字段  const string  MapName= "NewCotroller"; 
	  则路径规则变为：api/NewCotroller/方法名称，此时控制器名称被替换  
	  
	  
	  
	 
	
   

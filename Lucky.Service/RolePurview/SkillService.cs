﻿// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： SkillRespository.cs
// 项目名称： 
// 创建时间：2014/10/28
// 负责人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Lucky.Core;
using Lucky.Core.Data;
using Lucky.Entity;
using Lucky.IService;
namespace Lucky.Service
{
    public  class SkillService  :EntityRepository< Skill>,ISkillService
    {
      public SkillService(IHrDbContext context):base(context)
        {
            
        }
				
    }
}
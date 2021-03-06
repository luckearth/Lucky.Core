﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Core.Data.UnitOfWork;
using Lucky.Entity;


namespace Lucky.IService
{
    public interface INewsContext : IMainContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Link> Links { get; set; }
        DbSet<NewsArticle> NewsArticles { get; set; }
        DbSet<NewsArticleText> NewsArticleTexts { get; set; }
        
    }
}

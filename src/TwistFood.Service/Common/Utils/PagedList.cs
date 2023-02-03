using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwistFood.Service.Common.Utils
{
    public class PagedList<T>:List<T>
    {
        public PagenationMetaData MetaData { get; set; } = default!;

        public PagedList(List<T> items, int count, PagenationParams @params) 
        {
            MetaData = new PagenationMetaData(count, @params.PageSize, @params.PageNumber);
            AddRange(items);    
        }  
        public async static Task<PagedList<T>> ToPagedListAsync(IQueryable<T> query, PagenationParams @params)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((@params.PageNumber-1)*@params.PageSize)
                                    .Take(@params.PageSize).ToListAsync();
            return new PagedList<T>(items, count, @params);
        }
        public async static Task<PagedList<T>> ListToPagedListAsync(List<T> list, PagenationParams @params)
        {
            var count = list.Count();
            return new PagedList<T>(list, count, @params);  
        }
    }
}

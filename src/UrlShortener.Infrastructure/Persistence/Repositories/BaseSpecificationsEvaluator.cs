using Project.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Project.Infrastructure.Persistence.Repositories
{
    public static class BaseSpecificationsEvaluator<T> where T : class
    {


        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, BaseSpecifications<T> specification)
        {


            var query = InputQuery;




            if (specification.Criteria != null)
            {

                query = query.Where(specification.Criteria);
            }



            foreach (var item in specification.IncludeExpressions)
            {

                query = query.Include(item);

            }


            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            else if (specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);
        


                query = query.Skip(specification.Skip).Take(specification.Take);
    

            return query;



        }

    }
}

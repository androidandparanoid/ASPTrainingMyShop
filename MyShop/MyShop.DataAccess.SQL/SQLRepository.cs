using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    //3. Make your class public and use your interface, base it off your baseentity
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context; //context
        internal DbSet<T> dbSet; //underlying table that you need to access

        //6. Create a construct to pass data context

        public SQLRepository(DataContext context)
        {
            this.context = context;
            //7. Call the table by passing in the context and model you want to work in
            this.dbSet = context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            //throw new NotImplementedException();
            return dbSet;
        }

        public void Commit()
        {
            //throw new NotImplementedException();
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            //throw new NotImplementedException();
            var t = Find(Id);
            //once found and attached to model it can be removed
            if(context.Entry(t).State == EntityState.Detached)            
                dbSet.Attach(t);
            
            dbSet.Remove(t);
        }

        public T Find(string Id)
        {
            //throw new NotImplementedException();
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            //throw new NotImplementedException();
            dbSet.Add(t);

        }

        public void Update(T t)
        {
            //throw new NotImplementedException();
            dbSet.Attach(t);
            context.Entry(t).State = EntityState.Modified;

        }
    }
}

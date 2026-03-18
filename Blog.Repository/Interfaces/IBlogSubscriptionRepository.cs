using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.Interfaces;

namespace Blog.Repository.Interfaces
{
    public interface IBlogSubscriptionRepository : IRepository<BlogSubscription, long> { }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Service.Intefaces;

namespace Blog.Service.Commons
{
    public class BlogTagService : Service<BlogTag, long>, IBlogTagService
    {
        private readonly IRepository<BlogTag, long> _repository;

        public BlogTagService(IRepository<BlogTag, long> repository) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}

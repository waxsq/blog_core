using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Commons;
using Blog.Core.Entities;
using Blog.Core.Entities.Vo.Post;
using Blog.Core.Interfaces;
using Blog.Repository.Interfaces;
using Blog.Service.Intefaces;

namespace Blog.Service.Commons
{
    public class BlogPostService : Service<BlogPost, long>, IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IMapper _mapper;
        public BlogPostService(IBlogPostRepository blogPostRepository, IMapper mapper) : base(blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
            _mapper = mapper;
        }

        public Task<PageReponse<PostTablePageVo>> QueryPage(PostTableQueryVo vo)
        {
            return _blogPostRepository.QueryPageAsync(vo);
        }
    }
}

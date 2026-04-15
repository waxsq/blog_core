using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.Records;
using MediatR;
using SqlSugar;

namespace Blog.Core.RequestHandlers
{
    public class CategoryCountRequestHandler : IRequestHandler<CategoryCountRecord>
    {
        private readonly ISqlSugarClient _db;
        public CategoryCountRequestHandler(ISqlSugarClient db)
        {
            _db = db;
        }
        public Task Handle(CategoryCountRecord request, CancellationToken cancellationToken)
        {
            var count = request.Count;
            var Ids = request.Ids;
            _db.Updateable<BlogCategory>().SetColumns(c => c.RefCount == c.RefCount + count).Where(c => Ids.Contains(c.BlogCategoryId) && (c.RefCount + count) >= 0).ExecuteCommand();
            return Task.CompletedTask;
        }
    }
}

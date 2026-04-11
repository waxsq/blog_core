using Blog.Core.Entities;
using Blog.Core.Records;
using MediatR;
using SqlSugar;

namespace Blog.Core.RequestHandlers
{
    public class TagCountRequestHandler : IRequestHandler<TagCountRecord>
    {
        private readonly ISqlSugarClient _db;
        public TagCountRequestHandler(ISqlSugarClient db)
        {
            _db = db;
        }
        public Task Handle(TagCountRecord request, CancellationToken cancellationToken)
        {
            var tags = request.Tags;
            var count = request.Count;
            _db.Updateable<BlogTag>().UpdateColumns(t => t.RefCount + count)
                .Where(t => tags.Contains(t.BlogTagId) && (t.RefCount + count) >= 0)
                .ExecuteCommand();
            return Task.CompletedTask;
        }
    }
}
